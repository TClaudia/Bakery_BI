using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BakeryBI.Data;
using BakeryBI.Utils;

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

            InitializeMapComponents();
            InitializeProductMapComponents();
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
                MessageBox.Show($"Error loading data: {ex.Message}\n\nPlease ensure 'bakery_sales_cleaned_locations.csv' is in the application folder.",
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

            // Update tabs - FĂRĂ DUPLICATE!
            UpdateSalesOverTimeTab();
            UpdateMaxMinProductsTab();
            UpdateFutureSalesEstimationTab();
            UpdateEvolutionOfProfitsTab();

            // Update maps
            UpdateMapWithCurrentData();
            UpdateProductMapWithCurrentData();
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
            if (filteredData == null || !filteredData.Any()) return;

            var productSales = filteredData
                .GroupBy(r => r.ProductName)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalSales = g.Sum(r => r.FinalAmount)
                })
                .OrderByDescending(p => p.TotalSales)
                .ToList();

            if (!productSales.Any()) return;

            // Update MAX/MIN labels
            var maxProduct = productSales.First();
            var minProduct = productSales.Last();
            lblMaxProduct.Text = $"MAX: {maxProduct.Product} - ${maxProduct.TotalSales:N2}";
            lblMinProduct.Text = $"MIN: {minProduct.Product} - ${minProduct.TotalSales:N2}";

            // Clear chart
            chartMaxMinProducts.Series.Clear();

            // Configure chart area
            if (chartMaxMinProducts.ChartAreas.Count > 0)
            {
                var chartArea = chartMaxMinProducts.ChartAreas[0];
                chartArea.AxisX.Title = "Product";
                chartArea.AxisY.Title = "Sales Amount ($)";
                chartArea.AxisX.Interval = 1;
                chartArea.AxisX.LabelStyle.Angle = -45;
                chartArea.BackColor = Color.LightYellow;

                // Clear custom labels
                chartArea.AxisX.CustomLabels.Clear();
            }

            // Create single series for ALL products
            var series = new Series
            {
                Name = "Sales",
                ChartType = SeriesChartType.Column
            };

            // Add ALL products with INDEX as X value (not product name!)
            for (int i = 0; i < productSales.Count; i++)
            {
                var product = productSales[i];

                // Use INDEX i as X value, not product name!
                int pointIndex = series.Points.AddXY(i, product.TotalSales);
                var point = series.Points[pointIndex];

                // Color coding: Green for MAX, Red for MIN, Blue for others
                if (product.Product == maxProduct.Product)
                {
                    point.Color = Color.Green;
                    point.BorderWidth = 3;
                    point.BorderColor = Color.DarkGreen;
                }
                else if (product.Product == minProduct.Product)
                {
                    point.Color = Color.Red;
                    point.BorderWidth = 3;
                    point.BorderColor = Color.DarkRed;
                }
                else
                {
                    point.Color = Color.SteelBlue;
                }

                // Add custom label for product name
                var chartArea = chartMaxMinProducts.ChartAreas[0];
                CustomLabel label = new CustomLabel
                {
                    FromPosition = i - 0.5,
                    ToPosition = i + 0.5,
                    Text = product.Product
                };
                chartArea.AxisX.CustomLabels.Add(label);
            }

            chartMaxMinProducts.Series.Add(series);

            // Set axis range
            if (chartMaxMinProducts.ChartAreas.Count > 0)
            {
                var chartArea = chartMaxMinProducts.ChartAreas[0];
                chartArea.AxisX.Minimum = -0.5;
                chartArea.AxisX.Maximum = productSales.Count - 0.5;
            }

            // Configure legend
            if (chartMaxMinProducts.Legends.Count > 0)
            {
                chartMaxMinProducts.Legends[0].Docking = Docking.Top;
                chartMaxMinProducts.Legends[0].Alignment = StringAlignment.Center;
            }

            // Update data grid
            dgvProductSales.DataSource = productSales.Select(p => new
            {
                ProductName = p.Product,
                TotalSales = p.TotalSales
            }).ToList();

            if (dgvProductSales.Columns.Contains("TotalSales"))
            {
                dgvProductSales.Columns["TotalSales"].DefaultCellStyle.Format = "C2";
            }

            // Update product map
            UpdateProductMapWithCurrentData();
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

        private void chartSalesOverTime_Click(object sender, EventArgs e)
        {

        }
    }
}