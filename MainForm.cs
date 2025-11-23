using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BakeryBI.Data;
using BakeryBI.Utils;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace BakeryBI
{
    public partial class MainForm : Form
    {
        private List<SalesRecord> allSalesData;
        private List<SalesRecord> filteredData;
        private SalesDataLoader dataLoader;
        private List<string> selectedClientTypes = new List<string>();
        private List<string> selectedStoreNames = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            dataLoader = new SalesDataLoader();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeMaxMinProductsChart();
            InitializeSalesOverTimeChart();
            LoadData();
            PopulateFilters();
            ApplyFilters();
            InitializeCustomControlsAndEventsForSalesAnalysis();
        }

        private void LoadData()
        {
            try
            {
                string csvPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/bakery_sales_cleaned_locations.csv");
                allSalesData = dataLoader.LoadFromCsv(csvPath);

                if (allSalesData == null || allSalesData.Count == 0)
                {
                    MessageBox.Show("No data loaded. Please check the CSV file.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Successfully loaded {allSalesData.Count} records!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\n\nPlease ensure 'bakery_sales_cleaned.csv' is in the application folder.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allSalesData = new List<SalesRecord>();
            }
        }

        private void PopulateFilters()
        {
            if (allSalesData == null || allSalesData.Count == 0)
                return;

            // Populate Store filter
            cmbStore.Items.Clear();
            cmbStore.Items.Add("All Stores");
            var stores = allSalesData.Select(r => r.StoreName).Distinct().OrderBy(s => s).ToList();
            foreach (var store in stores)
            {
                cmbStore.Items.Add(store);
            }
            cmbStore.SelectedIndex = 0;

            // Populate Product filter
            cmbProduct.Items.Clear();
            cmbProduct.Items.Add("All Products");
            var products = allSalesData.Select(r => r.ProductName).Distinct().OrderBy(p => p).ToList();
            foreach (var product in products)
            {
                cmbProduct.Items.Add(product);
            }
            cmbProduct.SelectedIndex = 0;

            // Initialize DateTimePickers
            var minDate = allSalesData.Min(r => r.TransactionDate);
            var maxDate = allSalesData.Max(r => r.TransactionDate);

            dtpFrom.MinDate = minDate;
            dtpFrom.MaxDate = maxDate;
            dtpFrom.Value = minDate;

            dtpTo.MinDate = minDate;
            dtpTo.MaxDate = maxDate;
            dtpTo.Value = maxDate;
        }

        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allSalesData == null || allSalesData.Count == 0)
                return;

            filteredData = allSalesData.ToList();

            // Apply Date Range filter
            DateTime dateFrom = dtpFrom.Value.Date;
            DateTime dateTo = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);
            filteredData = filteredData.Where(r => r.TransactionDate >= dateFrom && r.TransactionDate <= dateTo).ToList();

            // Apply Store filter
            if (cmbStore.SelectedIndex > 0)
            {
                string selectedStore = cmbStore.SelectedItem.ToString();
                filteredData = filteredData.Where(r => r.StoreName == selectedStore).ToList();
            }

            // Apply Product filter
            if (cmbProduct.SelectedIndex > 0)
            {
                string selectedProduct = cmbProduct.SelectedItem.ToString();
                filteredData = filteredData.Where(r => r.ProductName == selectedProduct).ToList();
            }

            if (allSalesData.Any())
            {
                SyncClientTypeCheckBoxStates();
                SyncStoreCheckBoxStates();
            }

            // Update both tabs
            UpdateSalesOverTimeTab();
            UpdateMaxMinProductsTab();
            UpdateFutureSalesEstimationTab();
            UpdateEvolutionOfProfitsTab();
        }

        #region Sales Over Time Tab

        private void UpdateSalesOverTimeTab()
        {
            UpdateSalesOverTimeChart();
            UpdateSalesOverTimeDataGrid();
        }

        private void UpdateSalesOverTimeChart()
        {
            chartSalesOverTime.Series["Sales"].Points.Clear();

            if (filteredData == null || filteredData.Count == 0)
                return;

            var timeData = filteredData
                .GroupBy(r => new { r.Year, r.Month, r.MonthName })
                .Select(g => new
                {
                    Period = $"{g.Key.MonthName} {g.Key.Year}",
                    TotalSales = g.Sum(r => r.FinalAmount),
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    SortKey = new DateTime(g.Key.Year, g.Key.Month, 1)
                })
                .OrderBy(x => x.SortKey)
                .ToList();

            // ADD EACH MONTH TO CHART
            foreach (var item in timeData)
            {
                int pointIndex = chartSalesOverTime.Series["Sales"].Points.AddXY(
                    item.SortKey.ToOADate(),
                    (double)item.TotalSales       // Cast to double
                );
                chartSalesOverTime.Series["Sales"].Points[pointIndex].Label = $"${item.TotalSales:N0}";
                chartSalesOverTime.Series["Sales"].Points[pointIndex].Font = new Font("Arial", 8F);
            }

            var chartArea = chartSalesOverTime.ChartAreas[0];
            chartArea.AxisX.Minimum = timeData.First().SortKey.ToOADate();
            chartArea.AxisX.Maximum = timeData.Last().SortKey.ToOADate();
            chartArea.RecalculateAxesScale();
        }

        private void UpdateSalesOverTimeDataGrid()
        {
            if (filteredData == null || filteredData.Count == 0)
            {
                dgvSalesTimeData.DataSource = null;
                return;
            }

            // Create DataTable with aggregated data (Month-Year, Total Sales)
            var timeData = filteredData
                .GroupBy(r => new { r.Year, r.Month, r.MonthName })
                .Select(g => new
                {
                    MonthYear = $"{g.Key.MonthName} {g.Key.Year}",
                    TotalSales = g.Sum(r => r.FinalAmount),
                    Year = g.Key.Year,
                    Month = g.Key.Month
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Month-Year", typeof(string));
            dt.Columns.Add("Total Sales", typeof(decimal));

            foreach (var item in timeData)
            {
                dt.Rows.Add(item.MonthYear, item.TotalSales);
            }

            dgvSalesTimeData.DataSource = dt;

            // Format currency column
            if (dgvSalesTimeData.Columns.Contains("Total Sales"))
                dgvSalesTimeData.Columns["Total Sales"].DefaultCellStyle.Format = "C2";
        }

        #endregion

        #region Max/Min Products Tab

        private void UpdateMaxMinProductsTab()
        {
            UpdateMaxMinProductsChart();
            UpdateMaxMinProductsDataGrid();
        }

        private void UpdateMaxMinProductsChart()
        {
            var salesSeries = chartMaxMinProducts.Series["Sales"];
            salesSeries.Points.Clear();
            salesSeries.ChartType = SeriesChartType.Column;

            if (filteredData == null || filteredData.Count == 0)
            {
                lblMaxProduct.Text = "MAX: No data";
                lblMinProduct.Text = "MIN: No data";
                return;
            }

            var productSales = filteredData
                .GroupBy(r => r.ProductName)
                .Select(g => new { Product = g.Key, TotalSales = g.Sum(r => r.FinalAmount) })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            if (productSales.Count == 0) return;

            var maxProduct = productSales.First();
            var minProduct = productSales.Last();

            lblMaxProduct.Text = $"MAX: {maxProduct.Product} - ${maxProduct.TotalSales:N2}";
            lblMaxProduct.ForeColor = Color.Green;
            lblMinProduct.Text = $"MIN: {minProduct.Product} - ${minProduct.TotalSales:N2}";
            lblMinProduct.ForeColor = Color.Red;

            // Add all points
            int index = 0;
            foreach (var item in productSales)
            {
                int pointIndex = salesSeries.Points.AddXY(index, (double)item.TotalSales);

                if (item.Product == maxProduct.Product)
                {
                    salesSeries.Points[pointIndex].Color = Color.Green;
                    salesSeries.Points[pointIndex].Label = $"MAX\n${item.TotalSales:N0}";
                    salesSeries.Points[pointIndex].LabelForeColor = Color.DarkGreen;
                    salesSeries.Points[pointIndex].Font = new Font("Arial", 9, FontStyle.Bold);
                }
                else if (item.Product == minProduct.Product)
                {
                    salesSeries.Points[pointIndex].Color = Color.Red;
                    salesSeries.Points[pointIndex].Label = $"MIN\n${item.TotalSales:N0}";
                    salesSeries.Points[pointIndex].LabelForeColor = Color.DarkRed;
                    salesSeries.Points[pointIndex].Font = new Font("Arial", 9, FontStyle.Bold);
                }
                else
                {
                    salesSeries.Points[pointIndex].Color = Color.SteelBlue;
                }
                index++;
            }

            var chartArea = chartMaxMinProducts.ChartAreas[0];
            chartArea.AxisX.CustomLabels.Clear();

            for (int i = 0; i < productSales.Count; i++)
            {
                CustomLabel label = new CustomLabel();
                label.FromPosition = i - 0.5;
                label.ToPosition = i + 0.5;
                label.Text = productSales[i].Product;  // Set product name
                chartArea.AxisX.CustomLabels.Add(label);
            }

            chartArea.AxisX.Minimum = -0.5;
            chartArea.AxisX.Maximum = productSales.Count - 0.5;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            chartArea.RecalculateAxesScale();
            chartMaxMinProducts.Invalidate();
        }

        private void UpdateMaxMinProductsDataGrid()
        {
            if (filteredData == null || filteredData.Count == 0)
            {
                dgvProductSales.DataSource = null;
                return;
            }

            // Create DataTable with aggregated product sales
            var productSales = filteredData
                .GroupBy(r => r.ProductName)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalSales = g.Sum(r => r.FinalAmount)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Product Name", typeof(string));
            dt.Columns.Add("Total Sales", typeof(decimal));

            foreach (var item in productSales)
            {
                dt.Rows.Add(item.Product, item.TotalSales);
            }

            dgvProductSales.DataSource = dt;


            dgvProductSales.ColumnHeadersVisible = true;

            // Format currency column
            if (dgvProductSales.Columns.Contains("Total Sales"))
                dgvProductSales.Columns["Total Sales"].DefaultCellStyle.Format = "C2";
        }
        #endregion

        //Sales analysis

        private void RenderSalesEstimationChart(List<SalesRecord> filteredData, int forecastMonths)
        {
            // Prepare for fresh redraw of the chart
            chartFutureSalesEstimation.Series.Clear();
            chartFutureSalesEstimation.ChartAreas.Clear();

            // Calculate actual monthly sales for both chart and table
            var monthlySalesSummary = filteredData
                // Groups data per month by considering all transactions for the month are associated to the first day of the month in which they occured
                .GroupBy(x => new DateTime(x.TransactionDate.Year, x.TransactionDate.Month, 1))
                //Output is sorted chronologically
                .OrderBy(x => x.Key)
                // TotalSales per month is calculated by adding all the values for the month in the FinalAmount column
                .Select(x => new
                {
                    Month = x.Key,
                    TotalSales = x.Sum(x => x.FinalAmount)
                })
                .ToList();

            // Populate Data Table (dgvSalesData)
            dgvSalesData.DataSource = monthlySalesSummary;
            dgvSalesData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            if (!monthlySalesSummary.Any()) return;

            // Chart Setup
            var chartArea = new ChartArea("SalesArea");
            chartArea.AxisX.Title = "Month";
            chartArea.AxisY.Title = "Total Sales (Revenue)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
            chartArea.AxisX.LabelStyle.Format = "MMM yy";
            chartFutureSalesEstimation.ChartAreas.Add(chartArea);

            // Get forecast and trend points
            var trendAndForecastPoints = SalesUtility.CalculateTrendAndForecast(filteredData, forecastMonths);

            // Actual Sales Series (Column series)
            var actualSeries = new Series("Actual Sales")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.LightBlue,
                XValueType = ChartValueType.DateTime
            };
            monthlySalesSummary.ForEach(p => actualSeries.Points.AddXY(p.Month.ToOADate(), (double)p.TotalSales));
            chartFutureSalesEstimation.Series.Add(actualSeries);

            // Trend and Forecast Series (Line series)
            var trendSeries = new Series("Trend & Forecast")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                BorderWidth = 3,
                XValueType = ChartValueType.DateTime
            };

            foreach (var p in trendAndForecastPoints)
            {
                System.Windows.Forms.DataVisualization.Charting.DataPoint chartPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(p.Date.ToOADate(), (double)p.Value);
                if (p.IsForecast)
                {
                    chartPoint.BorderDashStyle = ChartDashStyle.Dash;
                    chartPoint.MarkerStyle = MarkerStyle.Circle;
                }
                trendSeries.Points.Add(chartPoint);
            }

            chartFutureSalesEstimation.Series.Add(trendSeries);
            chartFutureSalesEstimation.Titles.Clear();
            chartFutureSalesEstimation.Titles.Add("Future Sales Estimation (Monthly Revenue Trend)");
            chartFutureSalesEstimation.ChartAreas["SalesArea"].RecalculateAxesScale();
        }

        private void RenderProfitEvolutionChart(List<SalesRecord> rawData, List<string> selectedClientTypes)
        {
            // Prepare for fresh redraw of the chart
            chartEvolutionOfProfits.Series.Clear();
            chartEvolutionOfProfits.ChartAreas.Clear();
            chartEvolutionOfProfits.Legends.Clear();

            // Applies global filters
            List<SalesRecord> basedFilteredData = this.filteredData.ToList();

            // Applies tab specific filters
            var fullyFilteredData = basedFilteredData
                .Where(x => selectedClientTypes.Contains(x.CustomerType))
                .Where(x => selectedStoreNames.Contains(x.StoreName))
                .ToList();

            var monthlyProfitSummary = fullyFilteredData
                // Groups the sales data by Month (all transactions are considered for the 1st of the source month) and Store Name
                .GroupBy(r => new { Date = new DateTime(r.TransactionDate.Year, r.TransactionDate.Month, 1), r.StoreName })
                // Orders the entries by Month
                .OrderBy(g => g.Key.Date)
                // Calculates the Profit by Month per Store
                .Select(g => new
                {
                    Month = g.Key.Date,
                    Store = g.Key.StoreName,
                    Profit = g.Sum(r => r.Profit)
                })
                .ToList();

            // Populate Data Table (dgvProfitData)
            dgvProfitData.DataSource = monthlyProfitSummary;
            dgvProfitData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            if (!monthlyProfitSummary.Any())
            {
                chartEvolutionOfProfits.Titles.Clear();
                chartEvolutionOfProfits.Titles.Add("No Data Available for Selected Client Types.");
                return;
            }

            // Chart Setup
            var chartArea = new ChartArea("ProfitArea");
            chartArea.AxisX.Title = "Month of Transaction";
            chartArea.AxisY.Title = "Profit ($\text{FinalAmount} - \text{TotalCost}$)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
            chartArea.AxisX.LabelStyle.Format = "MMM yy";
            chartEvolutionOfProfits.ChartAreas.Add(chartArea);
            chartEvolutionOfProfits.Legends.Add(new Legend("StoreLegend"));

            // Create Chart Series
            var storeGroups = monthlyProfitSummary.GroupBy(r => r.Store).ToList();

            foreach (var storeGroup in storeGroups)
            {
                var series = new Series(storeGroup.Key)
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.DateTime,
                    BorderWidth = 2
                };

                // Populate series (casting decimal Profit to double for charting)
                storeGroup.ToList().ForEach(p => series.Points.AddXY(p.Month.ToOADate(), (double)p.Profit));

                chartEvolutionOfProfits.Series.Add(series);
            }

            chartEvolutionOfProfits.Titles.Clear();
            chartEvolutionOfProfits.Titles.Add("Evolution of Monthly Profit by Store");
            chartEvolutionOfProfits.ChartAreas["ProfitArea"].RecalculateAxesScale();
        }
        private void InitializeSalesOverTimeChart()
        {
            chartSalesOverTime.Series.Clear();
            chartSalesOverTime.ChartAreas.Clear();

            // Create Chart Area
            ChartArea chartArea = new ChartArea("SalesArea");
            chartArea.AxisX.Title = "Time Period (Month-Year)";
            chartArea.AxisY.Title = "Sales Amount ($)";

            // Configure X-axis for DateTime
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
            chartArea.AxisX.LabelStyle.Format = "MMM yyyy";
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.BackColor = Color.WhiteSmoke;
            chartSalesOverTime.ChartAreas.Add(chartArea);

            // Create Series
            Series series = new Series("Sales");
            series.ChartType = SeriesChartType.Line;
            series.Color = Color.Green;
            series.BorderWidth = 3;
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 8;
            series.MarkerColor = Color.DarkGreen;
            series.XValueType = ChartValueType.DateTime;
            chartSalesOverTime.Series.Add(series);

            // Add Legend
            Legend legend = new Legend();
            legend.Docking = Docking.Top;
            chartSalesOverTime.Legends.Add(legend);
        }
        private void InitializeMaxMinProductsChart()
        {
            chartMaxMinProducts.Series.Clear();
            chartMaxMinProducts.ChartAreas.Clear();
            chartMaxMinProducts.Legends.Clear();

            ChartArea chartArea = new ChartArea("ProductArea");
            chartArea.AxisX.Title = "Product";
            chartArea.AxisY.Title = "Sales Amount ($)";


            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.NotSet;
            chartArea.AxisX.IsMarginVisible = true;

            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.IsStartedFromZero = true;
            chartArea.BackColor = Color.LightYellow;

            chartMaxMinProducts.ChartAreas.Add(chartArea);

            Series series = new Series("Sales");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SteelBlue;
            series.BorderWidth = 1;
            series["PointWidth"] = "0.9";  // Width of bars

            chartMaxMinProducts.Series.Add(series);

            Legend legend = new Legend("ProductLegend");
            legend.Docking = Docking.Top;
            chartMaxMinProducts.Legends.Add(legend);
        }
        private void InitializeCustomControlsAndEventsForSalesAnalysis()
        {
            cmbForecastMonths.Items.AddRange(Enumerable.Range(1, 12).Cast<object>().ToArray());
            cmbForecastMonths.SelectedIndex = 2;

            PopulateClientTypeFilters();
            InitializeDefaultStoreFiler();

            PopulateStoreFilters();

            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;

            cmbForecastMonths.SelectedIndexChanged += cmbForecastMonths_SelectedIndexChanged;
        }
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabFutureSalesEstimation)
            {
                cmbForecastMonths_SelectedIndexChanged(null, null);
            }
            else if (tabControl.SelectedTab == tabEvolutionOfProfits)
            {
                RenderProfitEvolutionChart(allSalesData, selectedClientTypes);
            }
        }
        private void cmbForecastMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbForecastMonths.SelectedItem != null && allSalesData.Any())
            {
                int forecastMonths = (int)cmbForecastMonths.SelectedItem;
                RenderSalesEstimationChart(allSalesData, forecastMonths);
            }
        }
        private void ClientTypeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string clientType = cb.Tag.ToString();

            if (cb.Checked)
            {
                if (!selectedClientTypes.Contains(clientType)) selectedClientTypes.Add(clientType);
            }
            else
            {
                selectedClientTypes.Remove(clientType);
            }

            RenderProfitEvolutionChart(allSalesData, selectedClientTypes);
        }

        private void StoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string storeName = cb.Tag.ToString();

            if (cb.Checked)
            {
                if (!selectedStoreNames.Contains(storeName)) selectedStoreNames.Add(storeName);
            }
            else selectedStoreNames.Remove(storeName);

            RenderProfitEvolutionChart(allSalesData, selectedClientTypes);
        }
        private void PopulateClientTypeFilters()
        {
            pnlClientTypeFilters.Controls.Clear();

            var clientTypes = allSalesData.Select(x => x.CustomerType).Distinct().OrderBy(x => x).ToList();

            int xOffset = 10;

            foreach (var clientType in clientTypes)
            {
                var cb = new CheckBox
                {
                    Text = clientType,
                    Tag = clientType,
                    Checked = true,
                    Location = new Point(xOffset, 5),
                    AutoSize = true
                };

                cb.CheckedChanged += ClientTypeCheckBox_CheckedChanged;

                pnlClientTypeFilters.Controls.Add(cb);
                xOffset += cb.Width + 10;
            }
            selectedClientTypes = clientTypes;
        }

        private void PopulateStoreFilters()
        {
            pnlStoreFilters.Controls.Clear();

            var storeNames = allSalesData.Select(x => x.StoreName).Distinct().OrderBy(x => x).ToList();

            const int ColumnCount = 5;
            const int Margin = 10;
            const int ControlHeight = 25;
            const int ColumnWidth = 150;

            int xOffset = Margin;
            int yOffset = Margin;

            for (int i = 0; i < storeNames.Count; i++)
            {
                string storeName = storeNames[i];

                var cb = new CheckBox
                {
                    Text = storeName,
                    Tag = storeName,
                    Checked = true,
                    Location = new Point(xOffset, yOffset),
                    Width = ColumnWidth - Margin,
                    AutoSize = false
                };

                cb.CheckedChanged += StoreCheckBox_CheckedChanged;

                pnlStoreFilters.Controls.Add(cb);

                if ((i + 1) % ColumnCount == 0)
                {
                    xOffset = Margin;
                    yOffset += ControlHeight + Margin;
                }
                else xOffset += ColumnWidth;
            }
            selectedStoreNames = storeNames;
        }
        private void InitializeDefaultStoreFiler()
        {
            if (allSalesData == null || !allSalesData.Any()) return;
            selectedStoreNames = allSalesData.Select(x => x.StoreName).Distinct().OrderBy(x => x).ToList();
        }

        private void SyncClientTypeCheckBoxStates()
        {
            // Iterate through all CheckBox controls in the filter panel
            foreach (CheckBox cb in pnlClientTypeFilters.Controls.OfType<CheckBox>())
            {
                string clientType = cb.Tag.ToString();

                // If the global list contains the client type, the box should be checked.
                cb.Checked = selectedClientTypes.Contains(clientType);
            }
        }
        private void SyncStoreCheckBoxStates()
        {
            if (filteredData == null || !filteredData.Any()) return;

            // 1. Determine which stores still exist in the data after primary filtering
            var storesInCurrentData = filteredData.Select(r => r.StoreName).Distinct().ToList();

            // 2. Iterate through all CheckBox controls in the filter panel (assuming pnlStoreFilters still exists)
            foreach (CheckBox cb in pnlStoreFilters.Controls.OfType<CheckBox>())
            {
                string storeName = cb.Tag.ToString();

                // Disable the checkbox if the store doesn't exist in the current filtered data, 
                // but keep the checked state based on the global filter list.
                cb.Enabled = storesInCurrentData.Contains(storeName);

                // Crucial: The CHECKED state must still reflect the user's manual selection (selectedStoreNames)
                cb.Checked = selectedStoreNames.Contains(storeName);

                // Optional: Add visual cue for disabled control
                if (!cb.Enabled)
                {
                    cb.ForeColor = Color.DarkGray;
                }
                else
                {
                    cb.ForeColor = Color.Black;
                }
            }
        }
        private void UpdateFutureSalesEstimationTab()
        {
            if (cmbForecastMonths.SelectedItem != null && filteredData.Any())
            {
                int forecastMonths = (int)cmbForecastMonths.SelectedItem;
                RenderSalesEstimationChart(filteredData, forecastMonths);
            }
        }

        private void UpdateEvolutionOfProfitsTab()
        {
            RenderProfitEvolutionChart(filteredData, selectedClientTypes);
        }

        private void exportToExcel3_Click(object sender, EventArgs e)

        {

            try

            {

                SaveFileDialog saveDialog = new SaveFileDialog

                {

                    Filter = "Excel Files|*.xlsx",

                    FileName = $"FutureSalesEstimation_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"

                };



                if (saveDialog.ShowDialog() == DialogResult.OK)

                {

                    ExportFutureSalesToExcel(saveDialog.FileName);

                    MessageBox.Show($"Data exported successfully to:\n{saveDialog.FileName}",

                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error",

                MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }



        private void ExportFutureSalesToExcel(string filePath)

        {

            if (filteredData == null || !filteredData.Any())

            {

                MessageBox.Show("No data available to export.", "Warning",

                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }



            int forecastMonths = cmbForecastMonths.SelectedItem != null

            ? (int)cmbForecastMonths.SelectedItem

            : 3;



            Excel.Application excelApp = null;

            Excel.Workbook workbook = null;

            try

            {

                excelApp = new Excel.Application();

                excelApp.Visible = false;

                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Add();



                // Sheet 1: Monthly Sales Data (Descriptive Analytics)

                Excel.Worksheet monthlySheet = (Excel.Worksheet)workbook.Worksheets[1];

                monthlySheet.Name = "Monthly Sales Data";



                // Headers

                ((Excel.Range)monthlySheet.Cells[1, 1]).Value2 = "Month";

                ((Excel.Range)monthlySheet.Cells[1, 2]).Value2 = "Total Sales";

                ((Excel.Range)monthlySheet.Cells[1, 3]).Value2 = "Transaction Count";



                // Style headers

                Excel.Range headerRange1 = monthlySheet.Range["A1", "C1"];

                headerRange1.Font.Bold = true;

                headerRange1.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);



                // Calculate monthly sales

                // NOTE: filteredData already contains data filtered by Date Range, Store, and Product (from global filters)

                var monthlySales = filteredData

          .GroupBy(x => new DateTime(x.TransactionDate.Year, x.TransactionDate.Month, 1))

          .OrderBy(x => x.Key)

          .Select(x => new

          {

              Month = x.Key,

              TotalSales = x.Sum(r => r.FinalAmount),

              TransactionCount = x.Count()

          })

          .ToList();



                // Populate data

                int row = 2;

                foreach (var item in monthlySales)

                {

                    ((Excel.Range)monthlySheet.Cells[row, 1]).Value2 = item.Month.ToString("MMM yyyy");

                    ((Excel.Range)monthlySheet.Cells[row, 2]).Value2 = (double)item.TotalSales;

                    ((Excel.Range)monthlySheet.Cells[row, 2]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)monthlySheet.Cells[row, 3]).Value2 = item.TransactionCount;

                    row++;

                }



                // Add summary statistics (Descriptive Analytics)

                int summaryRow = row + 2;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "SUMMARY STATISTICS";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Font.Bold = true;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Font.Size = 12;



                summaryRow++;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Total Sales:";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Sum(x => x.TotalSales);

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Font.Bold = true;



                summaryRow++;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Average Monthly Sales:";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Average(x => x.TotalSales);

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";



                summaryRow++;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Maximum Monthly Sales:";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Max(x => x.TotalSales);

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";



                summaryRow++;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Minimum Monthly Sales:";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Min(x => x.TotalSales);

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";



                summaryRow++;

                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Total Transactions:";

                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = monthlySales.Sum(x => x.TransactionCount);



                // Auto-fit columns

                monthlySheet.Columns.AutoFit();



                // Sheet 2: Forecast Data (Predictive Analytics)

                Excel.Worksheet forecastSheet = (Excel.Worksheet)workbook.Worksheets.Add();

                forecastSheet.Name = "Forecast Data";



                // Headers

                ((Excel.Range)forecastSheet.Cells[1, 1]).Value2 = "Date";

                ((Excel.Range)forecastSheet.Cells[1, 2]).Value2 = "Type";

                ((Excel.Range)forecastSheet.Cells[1, 3]).Value2 = "Sales Forecast";



                // Style headers

                Excel.Range headerRange2 = forecastSheet.Range["A1", "C1"];

                headerRange2.Font.Bold = true;

                headerRange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGreen);



                // Get forecast data

                // NOTE: Uses filteredData which respects all global filters (Date Range, Store, Product)

                var trendAndForecast = SalesUtility.CalculateTrendAndForecast(filteredData, forecastMonths);



                // Populate forecast data

                row = 2;

                foreach (var point in trendAndForecast)

                {

                    ((Excel.Range)forecastSheet.Cells[row, 1]).Value2 = point.Date;

                    ((Excel.Range)forecastSheet.Cells[row, 1]).NumberFormat = "MMM yyyy";

                    ((Excel.Range)forecastSheet.Cells[row, 2]).Value2 = point.IsForecast ? "Forecast" : "Historical Trend";

                    ((Excel.Range)forecastSheet.Cells[row, 3]).Value2 = (double)point.Value;

                    ((Excel.Range)forecastSheet.Cells[row, 3]).NumberFormat = "$#,##0.00";



                    // Highlight forecast rows

                    if (point.IsForecast)

                    {

                        Excel.Range forecastRowRange = forecastSheet.Range[forecastSheet.Cells[row, 1], forecastSheet.Cells[row, 3]];

                        forecastRowRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);

                        ((Excel.Range)forecastSheet.Cells[row, 2]).Font.Italic = true;

                    }

                    row++;

                }



                // Add forecast summary

                var forecastPoints = trendAndForecast.Where(p => p.IsForecast).ToList();

                if (forecastPoints.Any())

                {

                    summaryRow = row + 2;

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "FORECAST SUMMARY";

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Font.Bold = true;

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Font.Size = 12;



                    summaryRow++;

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "Forecast Period:";

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Value2 = $"{forecastMonths} months";



                    summaryRow++;

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "Total Forecasted Sales:";

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Value2 = (double)forecastPoints.Sum(p => p.Value);

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Font.Bold = true;



                    summaryRow++;

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "Average Monthly Forecast:";

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Value2 = (double)forecastPoints.Average(p => p.Value);

                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";

                }



                // Auto-fit columns

                forecastSheet.Columns.AutoFit();



                // Sheet 3: Raw Dataset (Underlying Data Used for Analysis)

                Excel.Worksheet rawDataSheet = (Excel.Worksheet)workbook.Worksheets.Add();

                rawDataSheet.Name = "Raw Dataset";



                // Add title and summary info

                ((Excel.Range)rawDataSheet.Cells[1, 1]).Value2 = "RAW DATASET - All Filtered Transaction Records";

                ((Excel.Range)rawDataSheet.Cells[1, 1]).Font.Bold = true;

                ((Excel.Range)rawDataSheet.Cells[1, 1]).Font.Size = 14;

                ((Excel.Range)rawDataSheet.Cells[2, 1]).Value2 = $"Total Records: {filteredData.Count:N0}";

                ((Excel.Range)rawDataSheet.Cells[2, 1]).Font.Bold = true;

                ((Excel.Range)rawDataSheet.Cells[3, 1]).Value2 = $"Export Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";



                // Headers - All fields from SalesRecord (starting at row 5)

                int headerRow = 5;

                int colIndex = 1;

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Customer ID";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Store Name";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Transaction Date";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Aisle";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Product Name";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Quantity";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Unit Price";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Total Amount";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Discount Amount";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Final Amount";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Loyalty Points";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Unit Cost";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Total Cost";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Profit";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Profit Margin";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Year";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Month";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Month Name";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Quarter";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Day of Week";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Week Number";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Customer Type";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Store City";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Store Country";



                int totalColumns = colIndex - 1;



                // Style headers

                Excel.Range headerRange3 = rawDataSheet.Range[rawDataSheet.Cells[headerRow, 1], rawDataSheet.Cells[headerRow, totalColumns]];

                headerRange3.Font.Bold = true;

                headerRange3.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGray);



                // Populate raw data - Export all filtered records

                // NOTE: This is the exact dataset used to generate the analytics above

                row = headerRow + 1;

                foreach (var record in filteredData.OrderBy(r => r.TransactionDate).ThenBy(r => r.StoreName))

                {

                    colIndex = 1;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.CustomerId;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.StoreName;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.TransactionDate;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "yyyy-mm-dd";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Aisle;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.ProductName;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Quantity;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.UnitPrice;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.TotalAmount;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.DiscountAmount;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.FinalAmount;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.LoyaltyPoints;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.UnitCost;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.TotalCost;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.Profit;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.ProfitMargin;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "0.00%";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Year;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Month;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.MonthName;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Quarter;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.DayOfWeek;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.WeekNumber;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.CustomerType;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.StoreCity ?? "";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.StoreCountry ?? "";

                    row++;

                }



                // Auto-fit columns

                rawDataSheet.Columns.AutoFit();



                // Save file

                workbook.SaveAs(filePath);

            }

            finally

            {

                // Clean up COM objects

                if (workbook != null)

                {

                    workbook.Close(false);

                    Marshal.ReleaseComObject(workbook);

                }

                if (excelApp != null)

                {

                    excelApp.Quit();

                    Marshal.ReleaseComObject(excelApp);

                }

            }

        }

        private void exportToExcel4_Click(object sender, EventArgs e)

        {

            try

            {

                SaveFileDialog saveDialog = new SaveFileDialog

                {

                    Filter = "Excel Files|*.xlsx",

                    FileName = $"EvolutionOfProfits_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"

                };



                if (saveDialog.ShowDialog() == DialogResult.OK)

                {

                    ExportProfitsToExcel(saveDialog.FileName);

                    MessageBox.Show($"Data exported successfully to:\n{saveDialog.FileName}",

                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error",

                MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }



        private void ExportProfitsToExcel(string filePath)

        {

            if (filteredData == null || !filteredData.Any())

            {

                MessageBox.Show("No data available to export.", "Warning",

                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }



            // Apply tab-specific filters

            // NOTE: filteredData already has global filters (Date Range, Store, Product) applied

            // Now we apply the tab-specific filters (Client Type checkboxes and Store checkboxes)

            var fullyFilteredData = filteredData

          .Where(x => selectedClientTypes.Contains(x.CustomerType))

          .Where(x => selectedStoreNames.Contains(x.StoreName))

          .ToList();



            if (!fullyFilteredData.Any())

            {

                MessageBox.Show("No data available for selected filters.", "Warning",

                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }



            Excel.Application excelApp = null;

            Excel.Workbook workbook = null;

            try

            {

                excelApp = new Excel.Application();

                excelApp.Visible = false;

                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Add();



                // Sheet 1: Monthly Profit by Store (Descriptive Analytics)

                Excel.Worksheet profitSheet = (Excel.Worksheet)workbook.Worksheets[1];

                profitSheet.Name = "Monthly Profit by Store";



                // Calculate monthly profit by store

                var monthlyProfit = fullyFilteredData

          .GroupBy(r => new { Date = new DateTime(r.TransactionDate.Year, r.TransactionDate.Month, 1), r.StoreName })

          .OrderBy(g => g.Key.Date)

          .ThenBy(g => g.Key.StoreName)

          .Select(g => new

          {

              Month = g.Key.Date,

              Store = g.Key.StoreName,

              Profit = g.Sum(r => r.Profit),

              Sales = g.Sum(r => r.FinalAmount),

              TransactionCount = g.Count()

          })

          .ToList();



                // Get unique stores and months for pivot structure

                var stores = monthlyProfit.Select(x => x.Store).Distinct().OrderBy(s => s).ToList();

                var months = monthlyProfit.Select(x => x.Month).Distinct().OrderBy(m => m).ToList();



                // Headers - First column is Month, then one column per store

                ((Excel.Range)profitSheet.Cells[1, 1]).Value2 = "Month";

                int col = 2;

                foreach (var store in stores)

                {

                    ((Excel.Range)profitSheet.Cells[1, col]).Value2 = store;

                    col++;

                }

                ((Excel.Range)profitSheet.Cells[1, col]).Value2 = "Total";



                // Style headers

                Excel.Range headerRange4 = profitSheet.Range[profitSheet.Cells[1, 1], profitSheet.Cells[1, col]];

                headerRange4.Font.Bold = true;

                headerRange4.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);



                // Populate data

                int row = 2;

                foreach (var month in months)

                {

                    ((Excel.Range)profitSheet.Cells[row, 1]).Value2 = month.ToString("MMM yyyy");

                    ((Excel.Range)profitSheet.Cells[row, 1]).NumberFormat = "MMM yyyy";



                    col = 2;

                    decimal monthTotal = 0;

                    foreach (var store in stores)

                    {

                        var storeData = monthlyProfit.FirstOrDefault(x => x.Month == month && x.Store == store);

                        decimal profit = storeData?.Profit ?? 0;

                        ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)profit;

                        ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";

                        monthTotal += profit;

                        col++;

                    }



                    // Total for the month

                    ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)monthTotal;

                    ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)profitSheet.Cells[row, col]).Font.Bold = true;

                    row++;

                }



                // Add totals row

                ((Excel.Range)profitSheet.Cells[row, 1]).Value2 = "TOTAL";

                ((Excel.Range)profitSheet.Cells[row, 1]).Font.Bold = true;

                col = 2;

                foreach (var store in stores)

                {

                    decimal storeTotal = monthlyProfit.Where(x => x.Store == store).Sum(x => x.Profit);

                    ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)storeTotal;

                    ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)profitSheet.Cells[row, col]).Font.Bold = true;

                    col++;

                }

                decimal grandTotal = monthlyProfit.Sum(x => x.Profit);

                ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)grandTotal;

                ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";

                ((Excel.Range)profitSheet.Cells[row, col]).Font.Bold = true;



                // Auto-fit columns

                profitSheet.Columns.AutoFit();



                // Sheet 2: Store Performance Summary (Descriptive Analytics)

                Excel.Worksheet summarySheet = (Excel.Worksheet)workbook.Worksheets.Add();

                summarySheet.Name = "Store Performance Summary";



                // Headers

                ((Excel.Range)summarySheet.Cells[1, 1]).Value2 = "Store";

                ((Excel.Range)summarySheet.Cells[1, 2]).Value2 = "Total Profit";

                ((Excel.Range)summarySheet.Cells[1, 3]).Value2 = "Total Sales";

                ((Excel.Range)summarySheet.Cells[1, 4]).Value2 = "Profit Margin %";

                ((Excel.Range)summarySheet.Cells[1, 5]).Value2 = "Avg Monthly Profit";

                ((Excel.Range)summarySheet.Cells[1, 6]).Value2 = "Transaction Count";



                // Style headers

                Excel.Range headerRange5 = summarySheet.Range["A1", "F1"];

                headerRange5.Font.Bold = true;

                headerRange5.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGreen);



                // Calculate store summaries

                var storeSummaries = monthlyProfit

          .GroupBy(x => x.Store)

          .Select(g => new

          {

              Store = g.Key,

              TotalProfit = g.Sum(x => x.Profit),

              TotalSales = g.Sum(x => x.Sales),

              AvgMonthlyProfit = g.Average(x => x.Profit),

              TransactionCount = g.Sum(x => x.TransactionCount),

              MonthCount = g.Count()

          })

          .OrderByDescending(x => x.TotalProfit)

          .ToList();



                // Populate summary data

                row = 2;

                foreach (var summary in storeSummaries)

                {

                    ((Excel.Range)summarySheet.Cells[row, 1]).Value2 = summary.Store;

                    ((Excel.Range)summarySheet.Cells[row, 2]).Value2 = (double)summary.TotalProfit;

                    ((Excel.Range)summarySheet.Cells[row, 2]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)summarySheet.Cells[row, 3]).Value2 = (double)summary.TotalSales;

                    ((Excel.Range)summarySheet.Cells[row, 3]).NumberFormat = "$#,##0.00";



                    decimal profitMargin = summary.TotalSales != 0

                    ? (summary.TotalProfit / summary.TotalSales) * 100

                    : 0;

                    ((Excel.Range)summarySheet.Cells[row, 4]).Value2 = (double)profitMargin;

                    ((Excel.Range)summarySheet.Cells[row, 4]).NumberFormat = "0.00%";



                    ((Excel.Range)summarySheet.Cells[row, 5]).Value2 = (double)summary.AvgMonthlyProfit;

                    ((Excel.Range)summarySheet.Cells[row, 5]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)summarySheet.Cells[row, 6]).Value2 = summary.TransactionCount;

                    row++;

                }



                // Add overall summary

                int summaryRow = row + 2;

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "OVERALL SUMMARY";

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Font.Bold = true;

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Font.Size = 12;



                summaryRow++;

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Total Profit (All Stores):";

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = (double)storeSummaries.Sum(x => x.TotalProfit);

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Font.Bold = true;



                summaryRow++;

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Total Sales (All Stores):";

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = (double)storeSummaries.Sum(x => x.TotalSales);

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";



                summaryRow++;

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Overall Profit Margin:";

                decimal overallMargin = storeSummaries.Sum(x => x.TotalSales) != 0

                ? (storeSummaries.Sum(x => x.TotalProfit) / storeSummaries.Sum(x => x.TotalSales)) * 100

                : 0;

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = (double)overallMargin;

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).NumberFormat = "0.00%";



                summaryRow++;

                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Best Performing Store:";

                var bestStore = storeSummaries.OrderByDescending(x => x.TotalProfit).First();

                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = $"{bestStore.Store} (${bestStore.TotalProfit:N2})";



                // Auto-fit columns

                summarySheet.Columns.AutoFit();



                // Sheet 3: Detailed Monthly Data

                Excel.Worksheet detailSheet = (Excel.Worksheet)workbook.Worksheets.Add();

                detailSheet.Name = "Detailed Monthly Data";



                // Headers

                ((Excel.Range)detailSheet.Cells[1, 1]).Value2 = "Month";

                ((Excel.Range)detailSheet.Cells[1, 2]).Value2 = "Store";

                ((Excel.Range)detailSheet.Cells[1, 3]).Value2 = "Profit";

                ((Excel.Range)detailSheet.Cells[1, 4]).Value2 = "Sales";

                ((Excel.Range)detailSheet.Cells[1, 5]).Value2 = "Transaction Count";



                // Style headers

                Excel.Range headerRange6 = detailSheet.Range["A1", "E1"];

                headerRange6.Font.Bold = true;

                headerRange6.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);



                // Populate detailed data

                row = 2;

                foreach (var item in monthlyProfit.OrderBy(x => x.Month).ThenBy(x => x.Store))

                {

                    ((Excel.Range)detailSheet.Cells[row, 1]).Value2 = item.Month.ToString("MMM yyyy");

                    ((Excel.Range)detailSheet.Cells[row, 2]).Value2 = item.Store;

                    ((Excel.Range)detailSheet.Cells[row, 3]).Value2 = (double)item.Profit;

                    ((Excel.Range)detailSheet.Cells[row, 3]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)detailSheet.Cells[row, 4]).Value2 = (double)item.Sales;

                    ((Excel.Range)detailSheet.Cells[row, 4]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)detailSheet.Cells[row, 5]).Value2 = item.TransactionCount;

                    row++;

                }



                // Auto-fit columns

                detailSheet.Columns.AutoFit();



                // Sheet 4: Raw Dataset (Underlying Data Used for Analysis)

                Excel.Worksheet rawDataSheet = (Excel.Worksheet)workbook.Worksheets.Add();

                rawDataSheet.Name = "Raw Dataset";



                // Add title and summary info

                ((Excel.Range)rawDataSheet.Cells[1, 1]).Value2 = "RAW DATASET - All Filtered Transaction Records";

                ((Excel.Range)rawDataSheet.Cells[1, 1]).Font.Bold = true;

                ((Excel.Range)rawDataSheet.Cells[1, 1]).Font.Size = 14;

                ((Excel.Range)rawDataSheet.Cells[2, 1]).Value2 = $"Total Records: {fullyFilteredData.Count:N0}";

                ((Excel.Range)rawDataSheet.Cells[2, 1]).Font.Bold = true;

                ((Excel.Range)rawDataSheet.Cells[3, 1]).Value2 = $"Export Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";



                // Headers - All fields from SalesRecord (starting at row 5)

                // StoreCity and StoreCountry are the last two columns

                int headerRow = 5;

                int colIndex = 1;

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Customer ID";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Store Name";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Transaction Date";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Aisle";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Product Name";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Quantity";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Unit Price";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Total Amount";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Discount Amount";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Final Amount";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Loyalty Points";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Unit Cost";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Total Cost";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Profit";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Profit Margin";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Year";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Month";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Month Name";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Quarter";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Day of Week";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Week Number";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Customer Type";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Store City";

                ((Excel.Range)rawDataSheet.Cells[headerRow, colIndex++]).Value2 = "Store Country";



                int totalColumns = colIndex - 1;



                // Style headers

                Excel.Range headerRange7 = rawDataSheet.Range[rawDataSheet.Cells[headerRow, 1], rawDataSheet.Cells[headerRow, totalColumns]];

                headerRange7.Font.Bold = true;

                headerRange7.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGray);



                // Populate raw data - Export all filtered records

                // NOTE: This is the exact dataset (fullyFilteredData) used to generate the analytics above

                // It includes both global filters AND tab-specific filters (Client Types & Store checkboxes)

                row = headerRow + 1;

                foreach (var record in fullyFilteredData.OrderBy(r => r.TransactionDate).ThenBy(r => r.StoreName))

                {

                    colIndex = 1;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.CustomerId;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.StoreName;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.TransactionDate;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "yyyy-mm-dd";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Aisle;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.ProductName;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Quantity;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.UnitPrice;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.TotalAmount;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.DiscountAmount;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.FinalAmount;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.LoyaltyPoints;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.UnitCost;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.TotalCost;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.Profit;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "$#,##0.00";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = (double)record.ProfitMargin;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex - 1]).NumberFormat = "0.00%";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Year;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Month;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.MonthName;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.Quarter;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.DayOfWeek;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.WeekNumber;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.CustomerType;

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.StoreCity ?? "";

                    ((Excel.Range)rawDataSheet.Cells[row, colIndex++]).Value2 = record.StoreCountry ?? "";

                    row++;

                }



                // Auto-fit columns

                rawDataSheet.Columns.AutoFit();



                // Save file

                workbook.SaveAs(filePath);

            }

            finally

            {

                // Clean up COM objects

                if (workbook != null)

                {

                    workbook.Close(false);

                    Marshal.ReleaseComObject(workbook);

                }

                if (excelApp != null)

                {

                    excelApp.Quit();

                    Marshal.ReleaseComObject(excelApp);

                }

            }

        }
    }
}