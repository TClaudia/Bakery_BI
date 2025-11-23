using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BakeryBI.Data;
using BakeryBI.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System.IO;
using OfficeOpenXml.Drawing;

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

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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



            using (ExcelPackage package = new ExcelPackage())

            {

                // Sheet 1: Monthly Sales Data (Descriptive Analytics)

                var monthlySheet = package.Workbook.Worksheets.Add("Monthly Sales Data");



                // Headers

                monthlySheet.Cells[1, 1].Value = "Month";

                monthlySheet.Cells[1, 2].Value = "Total Sales";

                monthlySheet.Cells[1, 3].Value = "Transaction Count";



                // Style headers

                monthlySheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;

                monthlySheet.Cells[1, 1, 1, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                monthlySheet.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);



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

                    monthlySheet.Cells[row, 1].Value = item.Month.ToString("MMM yyyy");

                    monthlySheet.Cells[row, 2].Value = (double)item.TotalSales;

                    monthlySheet.Cells[row, 2].Style.Numberformat.Format = "$#,##0.00";

                    monthlySheet.Cells[row, 3].Value = item.TransactionCount;

                    row++;

                }



                // Add summary statistics (Descriptive Analytics)

                int summaryRow = row + 2;

                monthlySheet.Cells[summaryRow, 1].Value = "SUMMARY STATISTICS";

                monthlySheet.Cells[summaryRow, 1].Style.Font.Bold = true;

                monthlySheet.Cells[summaryRow, 1].Style.Font.Size = 12;



                summaryRow++;

                monthlySheet.Cells[summaryRow, 1].Value = "Total Sales:";

                monthlySheet.Cells[summaryRow, 2].Value = (double)monthlySales.Sum(x => x.TotalSales);

                monthlySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";

                monthlySheet.Cells[summaryRow, 2].Style.Font.Bold = true;



                summaryRow++;

                monthlySheet.Cells[summaryRow, 1].Value = "Average Monthly Sales:";

                monthlySheet.Cells[summaryRow, 2].Value = (double)monthlySales.Average(x => x.TotalSales);

                monthlySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";



                summaryRow++;

                monthlySheet.Cells[summaryRow, 1].Value = "Maximum Monthly Sales:";

                monthlySheet.Cells[summaryRow, 2].Value = (double)monthlySales.Max(x => x.TotalSales);

                monthlySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";



                summaryRow++;

                monthlySheet.Cells[summaryRow, 1].Value = "Minimum Monthly Sales:";

                monthlySheet.Cells[summaryRow, 2].Value = (double)monthlySales.Min(x => x.TotalSales);

                monthlySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";



                summaryRow++;

                monthlySheet.Cells[summaryRow, 1].Value = "Total Transactions:";

                monthlySheet.Cells[summaryRow, 2].Value = monthlySales.Sum(x => x.TransactionCount);



                // Auto-fit columns

                monthlySheet.Cells.AutoFitColumns();



                // Sheet 2: Forecast Data (Predictive Analytics)

                var forecastSheet = package.Workbook.Worksheets.Add("Forecast Data");



                // Headers

                forecastSheet.Cells[1, 1].Value = "Date";

                forecastSheet.Cells[1, 2].Value = "Type";

                forecastSheet.Cells[1, 3].Value = "Sales Forecast";



                // Style headers

                forecastSheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;

                forecastSheet.Cells[1, 1, 1, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                forecastSheet.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);



                // Get forecast data

                // NOTE: Uses filteredData which respects all global filters (Date Range, Store, Product)

                var trendAndForecast = SalesUtility.CalculateTrendAndForecast(filteredData, forecastMonths);



                // Populate forecast data

                row = 2;

                foreach (var point in trendAndForecast)

                {

                    forecastSheet.Cells[row, 1].Value = point.Date;

                    forecastSheet.Cells[row, 1].Style.Numberformat.Format = "MMM yyyy";

                    forecastSheet.Cells[row, 2].Value = point.IsForecast ? "Forecast" : "Historical Trend";

                    forecastSheet.Cells[row, 3].Value = (double)point.Value;

                    forecastSheet.Cells[row, 3].Style.Numberformat.Format = "$#,##0.00";



                    // Highlight forecast rows

                    if (point.IsForecast)

                    {

                        forecastSheet.Cells[row, 1, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                        forecastSheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        forecastSheet.Cells[row, 2].Style.Font.Italic = true;

                    }

                    row++;

                }



                // Add forecast summary

                var forecastPoints = trendAndForecast.Where(p => p.IsForecast).ToList();

                if (forecastPoints.Any())

                {

                    summaryRow = row + 2;

                    forecastSheet.Cells[summaryRow, 1].Value = "FORECAST SUMMARY";

                    forecastSheet.Cells[summaryRow, 1].Style.Font.Bold = true;

                    forecastSheet.Cells[summaryRow, 1].Style.Font.Size = 12;



                    summaryRow++;

                    forecastSheet.Cells[summaryRow, 1].Value = "Forecast Period:";

                    forecastSheet.Cells[summaryRow, 2].Value = $"{forecastMonths} months";



                    summaryRow++;

                    forecastSheet.Cells[summaryRow, 1].Value = "Total Forecasted Sales:";

                    forecastSheet.Cells[summaryRow, 2].Value = (double)forecastPoints.Sum(p => p.Value);

                    forecastSheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";

                    forecastSheet.Cells[summaryRow, 2].Style.Font.Bold = true;



                    summaryRow++;

                    forecastSheet.Cells[summaryRow, 1].Value = "Average Monthly Forecast:";

                    forecastSheet.Cells[summaryRow, 2].Value = (double)forecastPoints.Average(p => p.Value);

                    forecastSheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";

                }



                // Add helper columns for charting (Historical and Forecast separated)

                forecastSheet.Cells[1, 4].Value = "Historical Sales";

                forecastSheet.Cells[1, 5].Value = "Forecast Sales";

                forecastSheet.Cells[1, 4, 1, 5].Style.Font.Bold = true;

                forecastSheet.Cells[1, 4, 1, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                forecastSheet.Cells[1, 4, 1, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);



                row = 2;

                foreach (var point in trendAndForecast)

                {

                    if (!point.IsForecast)

                    {

                        forecastSheet.Cells[row, 4].Value = (double)point.Value;

                        forecastSheet.Cells[row, 4].Style.Numberformat.Format = "$#,##0.00";

                    }

                    else

                    {

                        forecastSheet.Cells[row, 5].Value = (double)point.Value;

                        forecastSheet.Cells[row, 5].Style.Numberformat.Format = "$#,##0.00";

                    }

                    row++;

                }



                // Auto-fit columns

                forecastSheet.Cells.AutoFitColumns();



                // Sheet 3: Raw Dataset (Underlying Data Used for Analysis)

                var rawDataSheet = package.Workbook.Worksheets.Add("Raw Dataset");



                // Add title and summary info

                rawDataSheet.Cells[1, 1].Value = "RAW DATASET - All Filtered Transaction Records";

                rawDataSheet.Cells[1, 1].Style.Font.Bold = true;

                rawDataSheet.Cells[1, 1].Style.Font.Size = 14;

                rawDataSheet.Cells[2, 1].Value = $"Total Records: {filteredData.Count:N0}";

                rawDataSheet.Cells[2, 1].Style.Font.Bold = true;

                rawDataSheet.Cells[3, 1].Value = $"Export Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";



                // Headers - All fields from SalesRecord (starting at row 5)

                int headerRow = 5;

                int colIndex = 1;

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Customer ID";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Store Name";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Transaction Date";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Aisle";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Product Name";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Quantity";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Unit Price";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Total Amount";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Discount Amount";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Final Amount";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Loyalty Points";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Unit Cost";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Total Cost";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Profit";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Profit Margin";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Year";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Month";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Month Name";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Quarter";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Day of Week";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Week Number";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Customer Type";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Store City";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Store Country";



                int totalColumns = colIndex - 1;



                // Style headers

                rawDataSheet.Cells[headerRow, 1, headerRow, totalColumns].Style.Font.Bold = true;

                rawDataSheet.Cells[headerRow, 1, headerRow, totalColumns].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                rawDataSheet.Cells[headerRow, 1, headerRow, totalColumns].Style.Fill.BackgroundColor.SetColor(Color.LightGray);



                // Populate raw data - Export all filtered records

                // NOTE: This is the exact dataset used to generate the analytics above

                row = headerRow + 1;

                foreach (var record in filteredData.OrderBy(r => r.TransactionDate).ThenBy(r => r.StoreName))

                {

                    colIndex = 1;

                    rawDataSheet.Cells[row, colIndex++].Value = record.CustomerId;

                    rawDataSheet.Cells[row, colIndex++].Value = record.StoreName;

                    rawDataSheet.Cells[row, colIndex++].Value = record.TransactionDate;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "yyyy-mm-dd";

                    rawDataSheet.Cells[row, colIndex++].Value = record.Aisle;

                    rawDataSheet.Cells[row, colIndex++].Value = record.ProductName;

                    rawDataSheet.Cells[row, colIndex++].Value = record.Quantity;

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.UnitPrice;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.TotalAmount;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.DiscountAmount;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.FinalAmount;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = record.LoyaltyPoints;

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.UnitCost;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.TotalCost;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.Profit;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.ProfitMargin;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "0.00%";

                    rawDataSheet.Cells[row, colIndex++].Value = record.Year;

                    rawDataSheet.Cells[row, colIndex++].Value = record.Month;

                    rawDataSheet.Cells[row, colIndex++].Value = record.MonthName;

                    rawDataSheet.Cells[row, colIndex++].Value = record.Quarter;

                    rawDataSheet.Cells[row, colIndex++].Value = record.DayOfWeek;

                    rawDataSheet.Cells[row, colIndex++].Value = record.WeekNumber;

                    rawDataSheet.Cells[row, colIndex++].Value = record.CustomerType;

                    rawDataSheet.Cells[row, colIndex++].Value = record.StoreCity ?? "";

                    rawDataSheet.Cells[row, colIndex++].Value = record.StoreCountry ?? "";

                    row++;

                }



                // Auto-fit columns

                rawDataSheet.Cells.AutoFitColumns();



                // Create Charts Sheet

                var chartsSheet = package.Workbook.Worksheets.Add("Charts");



                // Chart 1: Monthly Sales Trend Line Chart

                var monthlyChart = chartsSheet.Drawings.AddChart("MonthlySalesChart", eChartType.Line);

                monthlyChart.Title.Text = "Monthly Sales Trend";

                monthlyChart.SetPosition(1, 0, 0, 0);

                monthlyChart.SetSize(800, 400);



                // Add data series for monthly sales

                var monthlySeries = monthlyChart.Series.Add(

                    monthlySheet.Cells[$"B2:B{monthlySales.Count + 1}"],  // Y-axis: Total Sales

                    monthlySheet.Cells[$"A2:A{monthlySales.Count + 1}"]   // X-axis: Month

                );

                monthlySeries.Header = "Total Sales";

                monthlyChart.YAxis.Format = "$#,##0";

                monthlyChart.XAxis.Title.Text = "Month";

                monthlyChart.YAxis.Title.Text = "Total Sales ($)";



                // Chart 2: Historical Trend vs Forecast Combined Chart

                var forecastChart = chartsSheet.Drawings.AddChart("ForecastChart", eChartType.Line);

                forecastChart.Title.Text = "Sales Trend and Forecast";

                forecastChart.SetPosition(1, 0, 450, 0);

                forecastChart.SetSize(800, 400);



                int totalRows = trendAndForecast.Count + 1; // +1 for header row



                // Add Historical Trend Series (using helper column D)

                var histSeries = forecastChart.Series.Add(

                    forecastSheet.Cells[$"D2:D{totalRows}"],  // Y-axis: Historical Sales (helper column)

                    forecastSheet.Cells[$"A2:A{totalRows}"]   // X-axis: Date

                );

                histSeries.Header = "Historical Trend";

                histSeries.Border.Fill.Color = System.Drawing.Color.Blue;



                // Add Forecast Series (using helper column E)

                var forecastSeries = forecastChart.Series.Add(

                    forecastSheet.Cells[$"E2:E{totalRows}"],  // Y-axis: Forecast Sales (helper column)

                    forecastSheet.Cells[$"A2:A{totalRows}"]   // X-axis: Date

                );

                forecastSeries.Header = "Forecast";

                forecastSeries.Border.Fill.Color = System.Drawing.Color.Red;

                forecastSeries.Border.LineStyle = eLineStyle.Dash;



                forecastChart.YAxis.Format = "$#,##0";

                forecastChart.XAxis.Title.Text = "Date";

                forecastChart.YAxis.Title.Text = "Sales Forecast ($)";

                forecastChart.Legend.Position = eLegendPosition.Bottom;



                // Save file

                FileInfo file = new FileInfo(filePath);

                package.SaveAs(file);

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



            using (ExcelPackage package = new ExcelPackage())

            {

                // Sheet 1: Monthly Profit by Store (Descriptive Analytics)

                var profitSheet = package.Workbook.Worksheets.Add("Monthly Profit by Store");



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

                profitSheet.Cells[1, 1].Value = "Month";

                int col = 2;

                foreach (var store in stores)

                {

                    profitSheet.Cells[1, col].Value = store;

                    col++;

                }

                profitSheet.Cells[1, col].Value = "Total";



                // Style headers

                profitSheet.Cells[1, 1, 1, col].Style.Font.Bold = true;

                profitSheet.Cells[1, 1, 1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                profitSheet.Cells[1, 1, 1, col].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);



                // Populate data

                int row = 2;

                foreach (var month in months)

                {

                    profitSheet.Cells[row, 1].Value = month.ToString("MMM yyyy");

                    profitSheet.Cells[row, 1].Style.Numberformat.Format = "MMM yyyy";



                    col = 2;

                    decimal monthTotal = 0;

                    foreach (var store in stores)

                    {

                        var storeData = monthlyProfit.FirstOrDefault(x => x.Month == month && x.Store == store);

                        decimal profit = storeData?.Profit ?? 0;

                        profitSheet.Cells[row, col].Value = (double)profit;

                        profitSheet.Cells[row, col].Style.Numberformat.Format = "$#,##0.00";

                        monthTotal += profit;

                        col++;

                    }



                    // Total for the month

                    profitSheet.Cells[row, col].Value = (double)monthTotal;

                    profitSheet.Cells[row, col].Style.Numberformat.Format = "$#,##0.00";

                    profitSheet.Cells[row, col].Style.Font.Bold = true;

                    row++;

                }



                // Add totals row

                profitSheet.Cells[row, 1].Value = "TOTAL";

                profitSheet.Cells[row, 1].Style.Font.Bold = true;

                col = 2;

                foreach (var store in stores)

                {

                    decimal storeTotal = monthlyProfit.Where(x => x.Store == store).Sum(x => x.Profit);

                    profitSheet.Cells[row, col].Value = (double)storeTotal;

                    profitSheet.Cells[row, col].Style.Numberformat.Format = "$#,##0.00";

                    profitSheet.Cells[row, col].Style.Font.Bold = true;

                    col++;

                }

                decimal grandTotal = monthlyProfit.Sum(x => x.Profit);

                profitSheet.Cells[row, col].Value = (double)grandTotal;

                profitSheet.Cells[row, col].Style.Numberformat.Format = "$#,##0.00";

                profitSheet.Cells[row, col].Style.Font.Bold = true;



                // Auto-fit columns

                profitSheet.Cells.AutoFitColumns();



                // Sheet 2: Store Performance Summary (Descriptive Analytics)

                var summarySheet = package.Workbook.Worksheets.Add("Store Performance Summary");



                // Headers

                summarySheet.Cells[1, 1].Value = "Store";

                summarySheet.Cells[1, 2].Value = "Total Profit";

                summarySheet.Cells[1, 3].Value = "Total Sales";

                summarySheet.Cells[1, 4].Value = "Profit Margin %";

                summarySheet.Cells[1, 5].Value = "Avg Monthly Profit";

                summarySheet.Cells[1, 6].Value = "Transaction Count";



                // Style headers

                summarySheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;

                summarySheet.Cells[1, 1, 1, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                summarySheet.Cells[1, 1, 1, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);



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

                    summarySheet.Cells[row, 1].Value = summary.Store;

                    summarySheet.Cells[row, 2].Value = (double)summary.TotalProfit;

                    summarySheet.Cells[row, 2].Style.Numberformat.Format = "$#,##0.00";

                    summarySheet.Cells[row, 3].Value = (double)summary.TotalSales;

                    summarySheet.Cells[row, 3].Style.Numberformat.Format = "$#,##0.00";



                    decimal profitMargin = summary.TotalSales != 0

                    ? (summary.TotalProfit / summary.TotalSales) * 100

                    : 0;

                    summarySheet.Cells[row, 4].Value = (double)profitMargin;

                    summarySheet.Cells[row, 4].Style.Numberformat.Format = "0.00%";



                    summarySheet.Cells[row, 5].Value = (double)summary.AvgMonthlyProfit;

                    summarySheet.Cells[row, 5].Style.Numberformat.Format = "$#,##0.00";

                    summarySheet.Cells[row, 6].Value = summary.TransactionCount;

                    row++;

                }



                // Add overall summary

                int summaryRow = row + 2;

                summarySheet.Cells[summaryRow, 1].Value = "OVERALL SUMMARY";

                summarySheet.Cells[summaryRow, 1].Style.Font.Bold = true;

                summarySheet.Cells[summaryRow, 1].Style.Font.Size = 12;



                summaryRow++;

                summarySheet.Cells[summaryRow, 1].Value = "Total Profit (All Stores):";

                summarySheet.Cells[summaryRow, 2].Value = (double)storeSummaries.Sum(x => x.TotalProfit);

                summarySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";

                summarySheet.Cells[summaryRow, 2].Style.Font.Bold = true;



                summaryRow++;

                summarySheet.Cells[summaryRow, 1].Value = "Total Sales (All Stores):";

                summarySheet.Cells[summaryRow, 2].Value = (double)storeSummaries.Sum(x => x.TotalSales);

                summarySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "$#,##0.00";



                summaryRow++;

                summarySheet.Cells[summaryRow, 1].Value = "Overall Profit Margin:";

                decimal overallMargin = storeSummaries.Sum(x => x.TotalSales) != 0

                ? (storeSummaries.Sum(x => x.TotalProfit) / storeSummaries.Sum(x => x.TotalSales)) * 100

                : 0;

                summarySheet.Cells[summaryRow, 2].Value = (double)overallMargin;

                summarySheet.Cells[summaryRow, 2].Style.Numberformat.Format = "0.00%";



                summaryRow++;

                summarySheet.Cells[summaryRow, 1].Value = "Best Performing Store:";

                var bestStore = storeSummaries.OrderByDescending(x => x.TotalProfit).First();

                summarySheet.Cells[summaryRow, 2].Value = $"{bestStore.Store} (${bestStore.TotalProfit:N2})";



                // Auto-fit columns

                summarySheet.Cells.AutoFitColumns();



                // Sheet 3: Detailed Monthly Data

                var detailSheet = package.Workbook.Worksheets.Add("Detailed Monthly Data");



                // Headers

                detailSheet.Cells[1, 1].Value = "Month";

                detailSheet.Cells[1, 2].Value = "Store";

                detailSheet.Cells[1, 3].Value = "Profit";

                detailSheet.Cells[1, 4].Value = "Sales";

                detailSheet.Cells[1, 5].Value = "Transaction Count";



                // Style headers

                detailSheet.Cells[1, 1, 1, 5].Style.Font.Bold = true;

                detailSheet.Cells[1, 1, 1, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                detailSheet.Cells[1, 1, 1, 5].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);



                // Populate detailed data

                row = 2;

                foreach (var item in monthlyProfit.OrderBy(x => x.Month).ThenBy(x => x.Store))

                {

                    detailSheet.Cells[row, 1].Value = item.Month.ToString("MMM yyyy");

                    detailSheet.Cells[row, 2].Value = item.Store;

                    detailSheet.Cells[row, 3].Value = (double)item.Profit;

                    detailSheet.Cells[row, 3].Style.Numberformat.Format = "$#,##0.00";

                    detailSheet.Cells[row, 4].Value = (double)item.Sales;

                    detailSheet.Cells[row, 4].Style.Numberformat.Format = "$#,##0.00";

                    detailSheet.Cells[row, 5].Value = item.TransactionCount;

                    row++;

                }



                // Auto-fit columns

                detailSheet.Cells.AutoFitColumns();



                // Sheet 4: Raw Dataset (Underlying Data Used for Analysis)

                var rawDataSheet = package.Workbook.Worksheets.Add("Raw Dataset");



                // Add title and summary info

                rawDataSheet.Cells[1, 1].Value = "RAW DATASET - All Filtered Transaction Records";

                rawDataSheet.Cells[1, 1].Style.Font.Bold = true;

                rawDataSheet.Cells[1, 1].Style.Font.Size = 14;

                rawDataSheet.Cells[2, 1].Value = $"Total Records: {fullyFilteredData.Count:N0}";

                rawDataSheet.Cells[2, 1].Style.Font.Bold = true;

                rawDataSheet.Cells[3, 1].Value = $"Export Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";



                // Headers - All fields from SalesRecord (starting at row 5)

                int headerRow = 5;

                int colIndex = 1;

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Customer ID";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Store Name";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Store City";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Store Country";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Transaction Date";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Aisle";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Product Name";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Quantity";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Unit Price";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Total Amount";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Discount Amount";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Final Amount";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Loyalty Points";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Unit Cost";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Total Cost";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Profit";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Profit Margin";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Year";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Month";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Month Name";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Quarter";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Day of Week";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Week Number";

                rawDataSheet.Cells[headerRow, colIndex++].Value = "Customer Type";



                int totalColumns = colIndex - 1;



                // Style headers

                rawDataSheet.Cells[headerRow, 1, headerRow, totalColumns].Style.Font.Bold = true;

                rawDataSheet.Cells[headerRow, 1, headerRow, totalColumns].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                rawDataSheet.Cells[headerRow, 1, headerRow, totalColumns].Style.Fill.BackgroundColor.SetColor(Color.LightGray);



                // Populate raw data - Export all filtered records

                // NOTE: This is the exact dataset (fullyFilteredData) used to generate the analytics above

                // It includes both global filters AND tab-specific filters (Client Types & Store checkboxes)

                row = headerRow + 1;

                foreach (var record in fullyFilteredData.OrderBy(r => r.TransactionDate).ThenBy(r => r.StoreName))

                {

                    colIndex = 1;

                    rawDataSheet.Cells[row, colIndex++].Value = record.CustomerId;

                    rawDataSheet.Cells[row, colIndex++].Value = record.StoreName;

                    rawDataSheet.Cells[row, colIndex++].Value = record.TransactionDate;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "yyyy-mm-dd";

                    rawDataSheet.Cells[row, colIndex++].Value = record.Aisle;

                    rawDataSheet.Cells[row, colIndex++].Value = record.ProductName;

                    rawDataSheet.Cells[row, colIndex++].Value = record.Quantity;

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.UnitPrice;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.TotalAmount;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.DiscountAmount;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.FinalAmount;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = record.LoyaltyPoints;

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.UnitCost;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.TotalCost;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.Profit;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "$#,##0.00";

                    rawDataSheet.Cells[row, colIndex++].Value = (double)record.ProfitMargin;

                    rawDataSheet.Cells[row, colIndex - 1].Style.Numberformat.Format = "0.00%";

                    rawDataSheet.Cells[row, colIndex++].Value = record.Year;

                    rawDataSheet.Cells[row, colIndex++].Value = record.Month;

                    rawDataSheet.Cells[row, colIndex++].Value = record.MonthName;

                    rawDataSheet.Cells[row, colIndex++].Value = record.Quarter;

                    rawDataSheet.Cells[row, colIndex++].Value = record.DayOfWeek;

                    rawDataSheet.Cells[row, colIndex++].Value = record.WeekNumber;

                    rawDataSheet.Cells[row, colIndex++].Value = record.CustomerType;

                    rawDataSheet.Cells[row, colIndex++].Value = record.StoreCity ?? "";

                    rawDataSheet.Cells[row, colIndex++].Value = record.StoreCountry ?? "";

                    row++;

                }



                // Auto-fit columns

                rawDataSheet.Cells.AutoFitColumns();



                // Create Charts Sheet

                var chartsSheet = package.Workbook.Worksheets.Add("Charts");



                // Chart 1: Profit Evolution by Store (Line Chart)

                var profitEvolutionChart = chartsSheet.Drawings.AddChart("ProfitEvolutionChart", eChartType.Line);

                profitEvolutionChart.Title.Text = "Monthly Profit Evolution by Store";

                profitEvolutionChart.SetPosition(1, 0, 0, 0);

                profitEvolutionChart.SetSize(800, 400);



                // Add series for each store

                int monthRowCount = months.Count;

                foreach (var store in stores)

                {

                    // Find column index for this store

                    int storeColIndex = stores.IndexOf(store) + 2; // +2 because column 1 is Month, stores start at column 2

                    var storeSeries = profitEvolutionChart.Series.Add(

                        profitSheet.Cells[2, storeColIndex, monthRowCount + 1, storeColIndex],  // Y-axis: Profit values

                        profitSheet.Cells[2, 1, monthRowCount + 1, 1]   // X-axis: Month

                    );

                    storeSeries.Header = store;

                }



                profitEvolutionChart.YAxis.Format = "$#,##0";

                profitEvolutionChart.XAxis.Title.Text = "Month";

                profitEvolutionChart.YAxis.Title.Text = "Profit ($)";

                profitEvolutionChart.Legend.Position = eLegendPosition.Right;



                // Chart 2: Store Performance Comparison (Bar Chart)

                var storePerformanceChart = chartsSheet.Drawings.AddChart("StorePerformanceChart", eChartType.ColumnClustered);

                storePerformanceChart.Title.Text = "Store Performance Comparison";

                storePerformanceChart.SetPosition(1, 0, 450, 0);

                storePerformanceChart.SetSize(800, 400);



                // Add series for Total Profit

                var totalProfitSeries = storePerformanceChart.Series.Add(

                    summarySheet.Cells[$"B2:B{storeSummaries.Count + 1}"],  // Y-axis: Total Profit

                    summarySheet.Cells[$"A2:A{storeSummaries.Count + 1}"]   // X-axis: Store Name

                );

                totalProfitSeries.Header = "Total Profit";

                totalProfitSeries.Fill.Color = System.Drawing.Color.LightBlue;



                // Add series for Average Monthly Profit

                var avgProfitSeries = storePerformanceChart.Series.Add(

                    summarySheet.Cells[$"E2:E{storeSummaries.Count + 1}"],  // Y-axis: Avg Monthly Profit

                    summarySheet.Cells[$"A2:A{storeSummaries.Count + 1}"]   // X-axis: Store Name

                );

                avgProfitSeries.Header = "Avg Monthly Profit";

                avgProfitSeries.Fill.Color = System.Drawing.Color.LightGreen;



                storePerformanceChart.YAxis.Format = "$#,##0";

                storePerformanceChart.XAxis.Title.Text = "Store";

                storePerformanceChart.YAxis.Title.Text = "Profit ($)";

                storePerformanceChart.Legend.Position = eLegendPosition.Bottom;



                // Save file

                FileInfo file = new FileInfo(filePath);

                package.SaveAs(file);

            }

        }
    }
}