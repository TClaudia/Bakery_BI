using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BakeryBI.Models;
using BakeryBI.Services;
using System.IO;
using OfficeOpenXml;
using BakeryBI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.Common;
using NuGet.Packaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BakeryBI
{
    public partial class Form1 : Form
    {
        private DataService dataService;
        private List<Sale> currentSales;
        private List<Sale> allSales;
        public Form1()
        {
            InitializeComponent();
            dataService = new DataService();

            // Set EPPlus 8.x license
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // No action needed
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // No action needed
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // EPPlus license (dacă nu e în constructor)
            try
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            }
            catch { }

            // ORDINEA E IMPORTANTĂ!
            LoadData();              // 1. ÎNTÂI încarcă datele
            InitializeFilters();     // 2. APOI inițializează filtrele
            InitializeCharts();      // 3. APOI chart-urile
        }




        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData();
            InitializeFilters();
            InitializeCharts();
        }

        private void LoadData()
        {
            try
            {
                // Path to CSV file
                string csvPath = Path.Combine(Application.StartupPath, "Data", "bakery_sales_cleaned.csv");

                if (!File.Exists(csvPath))
                {
                               return;
                }

                allSales = dataService.LoadSalesData(csvPath);
                currentSales = allSales;

                lblStatus.Text = $"Loaded {allSales.Count:N0} transactions";

                RefreshDataGrid();
                RefreshCharts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeFilters()
        {
            try
            {
                // Store filter
                cmbStore.Items.Clear();
                cmbStore.Items.Add("All Stores");

                var stores = dataService.GetUniqueStores();
               

                foreach (var store in stores)
                {
                    cmbStore.Items.Add(store);
                }
                cmbStore.SelectedIndex = 0;

                // Category filter
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");

                var categories = dataService.GetUniqueCategories();
                MessageBox.Show($"Found {categories.Count} categories"); // DEBUG

                foreach (var category in categories)
                {
                    cmbCategory.Items.Add(category);
                }
                cmbCategory.SelectedIndex = 0;

                // Customer Type filter
                cmbCustomerType.Items.Clear();
                cmbCustomerType.Items.Add("All Types");

                var types = dataService.GetCustomerTypes();

                foreach (var type in types)
                {
                    cmbCustomerType.Items.Add(type);
                }
                cmbCustomerType.SelectedIndex = 0;

                // Year filter
                cmbYear.Items.Clear();
                cmbYear.Items.Add("All Years");

                var years = dataService.GetUniqueYears();

                foreach (var year in years)
                {
                    cmbYear.Items.Add(year);
                }
                cmbYear.SelectedIndex = 0;

                // Month filter
                cmbMonth.Items.Clear();
                cmbMonth.Items.Add("All Months");

                string[] months = { "January", "February", "March", "April", "May", "June",
                          "July", "August", "September", "October", "November", "December" };

                for (int i = 0; i < months.Length; i++)
                {
                    cmbMonth.Items.Add($"{i + 1} - {months[i]}");
                }
                cmbMonth.SelectedIndex = 0;

   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing filters: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeCharts()
        {
            // Configure Chart 1 - Sales by Store (SUBPUNCT 1: Graphic representation)
            chartSalesByStore.Series.Clear();
            chartSalesByStore.Series.Add(new Series
            {
                Name = "Sales",
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(78, 205, 196),
                Font = new Font("Segoe UI", 10F)
            });
            chartSalesByStore.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartSalesByStore.ChartAreas[0].AxisX.Interval = 1;

            // Configure Chart 2 - Sales by Category (PIE - SUBPUNCT 1)
            chartSalesByCategory.Series.Clear();
            chartSalesByCategory.Series.Add(new Series
            {
                Name = "Category",
                ChartType = SeriesChartType.Pie,
                Font = new Font("Segoe UI", 9F)
            });
            chartSalesByCategory.Series[0]["PieLabelStyle"] = "Outside";

            // Configure Chart 3 - Monthly Trend (SUBPUNCT 1: Different periods)
            chartMonthlyTrend.Series.Clear();
            chartMonthlyTrend.Series.Add(new Series
            {
                Name = "Sales",
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(255, 107, 107),
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8
            });

            // Configure Chart 4 - Customer Type
            chartCustomerType.Series.Clear();
            chartCustomerType.Series.Add(new Series
            {
                Name = "Sales",
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(255, 195, 113)
            });
        }

        private void RefreshDataGrid()
        {
            dgvSales.DataSource = null;
            dgvSales.DataSource = currentSales;

            // Format columns
            if (dgvSales.Columns.Count > 0)
            {
                dgvSales.Columns["final_amount"].DefaultCellStyle.Format = "C2";
                dgvSales.Columns["profit"].DefaultCellStyle.Format = "C2";
                dgvSales.Columns["profit_margin"].DefaultCellStyle.Format = "N2";
                dgvSales.Columns["transaction_date"].DefaultCellStyle.Format = "yyyy-MM-dd";
            }
        }

        // SUBPUNCT 1: Graphic representation of sales by product type
        private void RefreshCharts()
        {
            RefreshSalesByStore();
            RefreshSalesByCategory();
            RefreshMonthlyTrend();
            RefreshCustomerType();
        }

        // SUBPUNCT 1 & 2: Sales by Store with MAX/MIN highlighting
        private void RefreshSalesByStore()
        {
            chartSalesByStore.Series[0].Points.Clear();

            var storeSales = currentSales
                .GroupBy(s => s.store_name)
                .Select(g => new
                {
                    Store = g.Key,
                    TotalSales = g.Sum(s => s.final_amount)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            if (!storeSales.Any()) return;

            // Find MAX and MIN (SUBPUNCT 2)
            decimal maxSales = storeSales.Max(x => x.TotalSales);
            decimal minSales = storeSales.Min(x => x.TotalSales);

            foreach (var item in storeSales)
            {
                int pointIndex = chartSalesByStore.Series[0].Points.AddXY(item.Store, item.TotalSales);
                DataPoint point = chartSalesByStore.Series[0].Points[pointIndex];

                // SUBPUNCT 2: Highlight MAX (Green) and MIN (Red)
                if (item.TotalSales == maxSales)
                {
                    point.Color = Color.Green;
                    point.Label = $"MAX\n${item.TotalSales:N0}";
                    point.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                else if (item.TotalSales == minSales)
                {
                    point.Color = Color.Red;
                    point.Label = $"MIN\n${item.TotalSales:N0}";
                    point.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                else
                {
                    point.Label = $"${item.TotalSales:N0}";
                }
            }
        }

        // SUBPUNCT 1: Sales by Category (type/subtype)
        private void RefreshSalesByCategory()
        {
            chartSalesByCategory.Series[0].Points.Clear();

            var categorySales = currentSales
                .GroupBy(s => s.aisle)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSales = g.Sum(s => s.final_amount)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            foreach (var item in categorySales)
            {
                int pointIndex = chartSalesByCategory.Series[0].Points.AddXY(item.Category, item.TotalSales);
                chartSalesByCategory.Series[0].Points[pointIndex].Label = $"${item.TotalSales:N0}";
                chartSalesByCategory.Series[0].Points[pointIndex].LegendText = $"{item.Category} (${item.TotalSales:N0})";
            }
        }

        // SUBPUNCT 1 & 2: Monthly Trend with MAX/MIN (different periods)
        private void RefreshMonthlyTrend()
        {
            chartMonthlyTrend.Series[0].Points.Clear();

            var monthlySales = currentSales
                .GroupBy(s => new { s.year, s.month })
                .Select(g => new
                {
                    Period = $"{g.Key.year}-{g.Key.month:00}",
                    Year = g.Key.year,
                    Month = g.Key.month,
                    TotalSales = g.Sum(s => s.final_amount)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            if (!monthlySales.Any()) return;

            // Find MAX and MIN (SUBPUNCT 2)
            decimal maxSales = monthlySales.Max(x => x.TotalSales);
            decimal minSales = monthlySales.Min(x => x.TotalSales);

            foreach (var item in monthlySales)
            {
                int pointIndex = chartMonthlyTrend.Series[0].Points.AddXY(item.Period, item.TotalSales);
                DataPoint point = chartMonthlyTrend.Series[0].Points[pointIndex];

                // SUBPUNCT 2: Highlight MAX and MIN
                if (item.TotalSales == maxSales)
                {
                    point.MarkerColor = Color.Green;
                    point.MarkerSize = 12;
                    point.Label = $"MAX\n${item.TotalSales:N0}";
                    point.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (item.TotalSales == minSales)
                {
                    point.MarkerColor = Color.Red;
                    point.MarkerSize = 12;
                    point.Label = $"MIN\n${item.TotalSales:N0}";
                    point.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }

            chartMonthlyTrend.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartMonthlyTrend.ChartAreas[0].AxisX.Interval = 1;
        }

        private void RefreshCustomerType()
        {
            chartCustomerType.Series[0].Points.Clear();

            var customerSales = currentSales
                .GroupBy(s => s.customer_type)
                .Select(g => new
                {
                    Type = g.Key,
                    TotalSales = g.Sum(s => s.final_amount)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();

            foreach (var item in customerSales)
            {
                int pointIndex = chartCustomerType.Series[0].Points.AddXY(item.Type, item.TotalSales);
                chartCustomerType.Series[0].Points[pointIndex].Label = $"${item.TotalSales:N0}";
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string store = cmbStore.SelectedItem?.ToString();
                string category = cmbCategory.SelectedItem?.ToString();
                string customerType = cmbCustomerType.SelectedItem?.ToString();

                int? year = null;
                if (cmbYear.SelectedIndex > 0)
                {
                    year = Convert.ToInt32(cmbYear.SelectedItem);
                }

                int? month = null;
                if (cmbMonth.SelectedIndex > 0)
                {
                    month = cmbMonth.SelectedIndex; // Index corresponds to month number
                }

                currentSales = dataService.FilterSales(store, category, customerType, year, month);

                lblStatus.Text = $"Filtered: {currentSales.Count:N0} transactions";

                RefreshDataGrid();
                RefreshCharts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbStore.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            cmbCustomerType.SelectedIndex = 0;
            cmbYear.SelectedIndex = 0;
            cmbMonth.SelectedIndex = 0;

            currentSales = allSales;
            lblStatus.Text = $"Showing all {allSales.Count:N0} transactions";

            RefreshDataGrid();
            RefreshCharts();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"BakerySales_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(saveDialog.FileName);
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

        private void ExportToExcel(string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                // Sheet 1: Filtered Data
                var sheet = package.Workbook.Worksheets.Add("Sales Data");

                // Headers
                for (int i = 0; i < dgvSales.Columns.Count; i++)
                {
                    sheet.Cells[1, i + 1].Value = dgvSales.Columns[i].HeaderText;
                    sheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // Data
                for (int i = 0; i < currentSales.Count; i++)
                {
                    var sale = currentSales[i];
                    sheet.Cells[i + 2, 1].Value = sale.customer_id;
                    sheet.Cells[i + 2, 2].Value = sale.store_name;
                    sheet.Cells[i + 2, 3].Value = sale.transaction_date;
                    sheet.Cells[i + 2, 4].Value = sale.aisle;
                    sheet.Cells[i + 2, 5].Value = sale.product_name;
                    sheet.Cells[i + 2, 6].Value = sale.quantity;
                    sheet.Cells[i + 2, 7].Value = (double)sale.final_amount;
                    sheet.Cells[i + 2, 8].Value = (double)sale.profit;
                }

                sheet.Cells.AutoFitColumns();

                FileInfo file = new FileInfo(filePath);
                package.SaveAs(file);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Empty - no action needed
        }
    }

}
