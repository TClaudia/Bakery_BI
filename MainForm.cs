using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BakeryBI.Data;
using BakeryBI.Utils;
using System.Windows.Forms.DataVisualization.Charting;

namespace BakeryBI
{
    public partial class MainForm : Form
    {
        private List<SalesRecord> allSalesData;
        private List<SalesRecord> filteredData;
        private SalesDataLoader dataLoader;
        private ForecastCalculator forecastCalculator;

        public MainForm()
        {
            InitializeComponent();
            dataLoader = new SalesDataLoader();
            forecastCalculator = new ForecastCalculator();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadData();
            SetupCharts();
            PopulateFilters();
            ApplyFilter();
        }

        private void LoadData()
        {
            try
            {
                string csvPath = System.IO.Path.Combine(Application.StartupPath, "bakery_sales_cleaned.csv");

                if (!System.IO.File.Exists(csvPath))
                {
                    csvPath = "bakery_sales_cleaned.csv";
                }

                allSalesData = dataLoader.LoadFromCsv(csvPath);

                if (allSalesData.Count == 0)
                {
                    MessageBox.Show("No data loaded from CSV file.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Successfully loaded {allSalesData.Count} records!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\n\nPlease ensure 'bakery_sales_cleaned.csv' is in the application folder.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allSalesData = new List<SalesRecord>();
            }
        }

        private void SetupCharts()
        {
            // Chart 1: Sales/Costs over time
            SetupSalesCostsOverTimeChart();

            // Chart 2: Max/Min highlighting
            SetupMaxMinChart();
        }

        private void SetupSalesCostsOverTimeChart()
        {
            chartSalesCosts.Series.Clear();
            chartSalesCosts.ChartAreas.Clear();
            chartSalesCosts.Legends.Clear();

            ChartArea chartArea = new ChartArea("TimeArea");
            chartArea.AxisX.Title = "Time Period (Month-Year)";
            chartArea.AxisY.Title = "Amount ($)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.BackColor = Color.Honeydew;
            chartSalesCosts.ChartAreas.Add(chartArea);

            // Sales line
            Series salesSeries = new Series("Sales")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Green,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 6,
                MarkerColor = Color.DarkGreen
            };
            chartSalesCosts.Series.Add(salesSeries);

            // Costs line
            Series costsSeries = new Series("Costs")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Square,
                MarkerSize = 6,
                MarkerColor = Color.DarkRed
            };
            chartSalesCosts.Series.Add(costsSeries);

            Legend legend = new Legend("Legend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center
            };
            chartSalesCosts.Legends.Add(legend);

            chartSalesCosts.Titles.Clear();
            chartSalesCosts.Titles.Add(new Title("Sales & Costs Over Time by Product Type",
                Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.DarkGreen));
        }

        private void SetupMaxMinChart()
        {
            chartProfitEvolution.Series.Clear();
            chartProfitEvolution.ChartAreas.Clear();
            chartProfitEvolution.Legends.Clear();

            ChartArea chartArea = new ChartArea("MaxMinArea");
            chartArea.AxisX.Title = "Product";
            chartArea.AxisY.Title = "Total Sales ($)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.BackColor = Color.LavenderBlush;
            chartProfitEvolution.ChartAreas.Add(chartArea);

            Series maxMinSeries = new Series("Sales")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SteelBlue,
                BorderWidth = 1,
                BorderColor = Color.DarkBlue
            };
            chartProfitEvolution.Series.Add(maxMinSeries);

            Legend legend = new Legend("Legend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center
            };
            chartProfitEvolution.Legends.Add(legend);

            chartProfitEvolution.Titles.Clear();
            chartProfitEvolution.Titles.Add(new Title("Product Sales - Max & Min Highlighted",
                Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue));
        }

        private void PopulateFilters()
        {
            if (allSalesData == null || allSalesData.Count == 0)
                return;

            // Populate Store filter
            cboStore.Items.Clear();
            cboStore.Items.Add("All Stores");
            var stores = allSalesData
                .Select(r => r.StoreName)
                .Where(s => !string.IsNullOrEmpty(s))
                .Distinct()
                .OrderBy(s => s);
            foreach (var store in stores)
                cboStore.Items.Add(store);
            cboStore.SelectedIndex = 0;

            // Populate Product filter
            cboProduct.Items.Clear();
            cboProduct.Items.Add("All Products");
            var products = allSalesData
                .Select(r => r.ProductName)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .OrderBy(p => p);
            foreach (var product in products)
                cboProduct.Items.Add(product);
            cboProduct.SelectedIndex = 0;

            // Populate Customer Type filter
            cboCustomerType.Items.Clear();
            cboCustomerType.Items.Add("All Types");
            cboCustomerType.Items.Add("Firm");
            cboCustomerType.Items.Add("Individual");
            cboCustomerType.SelectedIndex = 0;

            // Set date range
            if (allSalesData.Count > 0)
            {
                dtpStartDate.Value = allSalesData.Min(r => r.TransactionDate);
                dtpEndDate.Value = allSalesData.Max(r => r.TransactionDate);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboStore.SelectedIndex = 0;
            cboProduct.SelectedIndex = 0;
            cboCustomerType.SelectedIndex = 0;

            if (allSalesData != null && allSalesData.Count > 0)
            {
                dtpStartDate.Value = allSalesData.Min(r => r.TransactionDate);
                dtpEndDate.Value = allSalesData.Max(r => r.TransactionDate);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (allSalesData == null || allSalesData.Count == 0)
            {
                MessageBox.Show("No data available to filter.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            filteredData = allSalesData.AsEnumerable().ToList();

            // Apply store filter
            if (cboStore.SelectedIndex > 0)
            {
                string selectedStore = cboStore.SelectedItem.ToString();
                filteredData = filteredData.Where(r => r.StoreName == selectedStore).ToList();
            }

            // Apply product filter
            if (cboProduct.SelectedIndex > 0)
            {
                string selectedProduct = cboProduct.SelectedItem.ToString();
                filteredData = filteredData.Where(r => r.ProductName == selectedProduct).ToList();
            }

            // Apply customer type filter
            if (cboCustomerType.SelectedIndex > 0)
            {
                string selectedType = cboCustomerType.SelectedItem.ToString();
                filteredData = filteredData.Where(r => r.CustomerType == selectedType).ToList();
            }

            // Apply date range filter
            filteredData = filteredData.Where(r =>
                r.TransactionDate.Date >= dtpStartDate.Value.Date &&
                r.TransactionDate.Date <= dtpEndDate.Value.Date).ToList();

            // Update UI - Only 2 charts needed
            UpdateDataGrid();
            UpdateSalesCostsOverTimeChart();  // Bullet 1
            UpdateMaxMinChart();               // Bullet 2
            UpdateForecast();                  // Bullet 3
        }

        private void UpdateDataGrid()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Store", typeof(string));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Final Amount", typeof(decimal));
            dt.Columns.Add("Profit", typeof(decimal));
            dt.Columns.Add("Customer Type", typeof(string));

            foreach (var record in filteredData)
            {
                dt.Rows.Add(
                    record.StoreName,
                    record.TransactionDate,
                    record.ProductName,
                    record.Quantity,
                    record.FinalAmount,
                    record.Profit,
                    record.CustomerType
                );
            }

            dgvSales.DataSource = dt;

            if (dgvSales.Columns.Contains("Final Amount"))
                dgvSales.Columns["Final Amount"].DefaultCellStyle.Format = "C2";
            if (dgvSales.Columns.Contains("Profit"))
                dgvSales.Columns["Profit"].DefaultCellStyle.Format = "C2";
        }

        // BULLET 1: Sales/Costs over time
        private void UpdateSalesCostsOverTimeChart()
        {
            chartSalesCosts.Series["Sales"].Points.Clear();
            chartSalesCosts.Series["Costs"].Points.Clear();

            if (filteredData == null || filteredData.Count == 0)
                return;

            var timeData = filteredData
                .GroupBy(r => new { r.Year, r.Month, r.MonthName })
                .Select(g => new
                {
                    Period = $"{g.Key.MonthName} {g.Key.Year}",
                    TotalSales = g.Sum(r => r.FinalAmount),
                    TotalCosts = g.Sum(r => r.TotalCost),
                    Year = g.Key.Year,
                    Month = g.Key.Month
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            foreach (var item in timeData)
            {
                chartSalesCosts.Series["Sales"].Points.AddXY(item.Period, item.TotalSales);
                chartSalesCosts.Series["Costs"].Points.AddXY(item.Period, item.TotalCosts);
            }
        }

        // BULLET 2: Max/Min highlighting
        private void UpdateMaxMinChart()
        {
            chartProfitEvolution.Series["Sales"].Points.Clear();

            if (filteredData == null || filteredData.Count == 0)
            {
                lblMaxProduct.Text = "MAX: No data";
                lblMinProduct.Text = "MIN: No data";
                lblForecast.Text = "No data for analysis";
                return;
            }

            var productSales = filteredData
                .GroupBy(r => r.ProductName)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalSales = g.Sum(r => r.FinalAmount)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            if (productSales.Count == 0)
                return;

            // Find max and min
            var maxProduct = productSales.First();
            var minProduct = productSales.Last();

            // Add all products to chart
            foreach (var item in productSales)
            {
                int pointIndex = chartProfitEvolution.Series["Sales"].Points.AddXY(item.Product, item.TotalSales);

                // Highlight MAX in green
                if (item.Product == maxProduct.Product)
                {
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Color = Color.Green;
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Label = $"MAX\n${item.TotalSales:N0}";
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].LabelForeColor = Color.DarkGreen;
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Font = new Font("Arial", 9, FontStyle.Bold);
                }
                // Highlight MIN in red
                else if (item.Product == minProduct.Product)
                {
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Color = Color.Red;
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Label = $"MIN\n${item.TotalSales:N0}";
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].LabelForeColor = Color.DarkRed;
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Font = new Font("Arial", 9, FontStyle.Bold);
                }
                else
                {
                    chartProfitEvolution.Series["Sales"].Points[pointIndex].Color = Color.SteelBlue;
                }
            }

            // Update labels
            lblMaxProduct.Text = $"🔼 MAX: {maxProduct.Product} - ${maxProduct.TotalSales:N2}";
            lblMinProduct.Text = $"🔽 MIN: {minProduct.Product} - ${minProduct.TotalSales:N2}";
        }

        // BULLET 3: Forecasting
        private void UpdateForecast()
        {
            if (filteredData == null || filteredData.Count == 0)
            {
                lblForecast.Text = "📊 Forecast: No data available";
                return;
            }

            // Get monthly sales data
            var monthlyData = filteredData
                .GroupBy(r => new { r.Year, r.Month })
                .Select(g => new
                {
                    Period = g.Key.Year * 12 + g.Key.Month,
                    TotalSales = g.Sum(r => r.FinalAmount)
                })
                .OrderBy(x => x.Period)
                .ToList();

            if (monthlyData.Count < 2)
            {
                lblForecast.Text = "📊 Forecast: Need at least 2 periods for trend analysis";
                return;
            }

            // Calculate forecast
            var salesValues = monthlyData.Select(m => m.TotalSales).ToList();
            var forecast = forecastCalculator.CalculateLinearForecast(salesValues);

            lblForecast.Text = $"📊 Next Period Forecast: ${forecast.ForecastedValue:N2} | " +
                             $"Trend: {forecast.TrendDirection} (${Math.Abs(forecast.Slope):N2}/period)";
        }
    }
}