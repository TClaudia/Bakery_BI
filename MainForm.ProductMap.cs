using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using BakeryBI.Data;
using System.Threading.Tasks;

namespace BakeryBI
{
    public partial class MainForm
    {
        private WebView2? webViewProductMap;
        private ProductLocationDataLoader? productLocationLoader;
        private bool isProductMapInitialized = false;

        private void InitializeProductMapComponents()
        {
            productLocationLoader = new ProductLocationDataLoader();

            webViewProductMap = new WebView2
            {
                Dock = DockStyle.Fill
            };

            var chart = chartMaxMinProducts;
            var dataGrid = dgvProductSales;
            var panelMaxMin = this.panelMaxMin;
            splitContainerMaxMin.Dock = DockStyle.Fill;

            splitContainerMaxMin.Panel1.Controls.Clear();
            splitContainerMaxMin.Panel2.Controls.Clear();

            
            splitContainerMaxMin.Orientation = Orientation.Horizontal;

            // Calculate 50% of the available height dynamically
            int defaultHeight = splitContainerMaxMin.Height > 0 ? splitContainerMaxMin.Height : 800;
            splitContainerMaxMin.SplitterDistance = defaultHeight / 2;

            splitContainerMaxMin.IsSplitterFixed = false; // Allow user to resize

            // Create top split container for Map + DataGrid
            var splitTop = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 1000,
                IsSplitterFixed = false
            };

            webViewProductMap.Dock = DockStyle.Fill;
            splitTop.Panel1.Controls.Add(webViewProductMap);

            dataGrid.Dock = DockStyle.Fill;
            splitTop.Panel2.Controls.Add(dataGrid);

            splitContainerMaxMin.Panel1.Controls.Add(splitTop);

            // Create a container panel for the bottom section
            var chartPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Add the Max/Min panel at the top
            panelMaxMin.Dock = DockStyle.Top;
            chartPanel.Controls.Add(panelMaxMin);

            chart.Dock = DockStyle.Fill;
            chartPanel.Controls.Add(chart);

            splitContainerMaxMin.Panel2.Controls.Add(chartPanel);

            //Adjust split distance after container is sized
            splitContainerMaxMin.SizeChanged += (s, e) =>
            {
                if (splitContainerMaxMin.Height > 0)
                {
                    // Set to 50% of total height
                    splitContainerMaxMin.SplitterDistance = splitContainerMaxMin.Height / 2;
                }
            };

            InitializeProductMapAsync();
        }

        private async void InitializeProductMapAsync()
        {
            if (webViewProductMap == null) return;

            try
            {
                await webViewProductMap.EnsureCoreWebView2Async(null);

                string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "product-map.html");

                if (!File.Exists(htmlPath))
                {
                    CreateProductMapHtmlFile(htmlPath);
                }

                webViewProductMap.CoreWebView2.Navigate(new Uri(htmlPath).AbsoluteUri);

                webViewProductMap.CoreWebView2.NavigationCompleted += async (s, e) =>
                {
                    isProductMapInitialized = true;

                    // Force map resize
                    await Task.Delay(100);
                    await webViewProductMap.CoreWebView2.ExecuteScriptAsync("map.invalidateSize();");

                    UpdateProductMapWithCurrentData();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing product map: {ex.Message}",
                    "Map Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CreateProductMapHtmlFile(string htmlPath)
        {
            string? resourcesDir = Path.GetDirectoryName(htmlPath);
            if (resourcesDir != null && !Directory.Exists(resourcesDir))
            {
                Directory.CreateDirectory(resourcesDir);
            }

            // Copy HTML from Resources folder
            string sourceHtml = @"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'/>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'/>
    <style>
        body { margin: 0; padding: 0; }
        #map { position: absolute; top: 0; bottom: 0; width: 100%; height: 100%; }
    </style>
</head>
<body>
    <div id='map'></div>
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <script>
        var map = L.map('map').setView([45.9432, 24.9668], 7);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap',
            maxZoom: 18
        }).addTo(map);
        var markersLayer = L.layerGroup().addTo(map);
        window.updateProductMapData = function(jsonData) {
            try { var data = JSON.parse(jsonData); } 
            catch(e) { console.error('Error:', e); }
        };
    </script>
</body>
</html>";

            File.WriteAllText(htmlPath, sourceHtml);
        }

        private async void UpdateProductMapWithCurrentData()
        {
            if (!isProductMapInitialized || webViewProductMap == null || productLocationLoader == null ||
                filteredData == null || !filteredData.Any())
                return;

            try
            {
                var productLocations = productLocationLoader.GetProductLocations(filteredData);

                // Add color for each location
                var locationsWithColors = productLocations.Select(loc => new
                {
                    loc.StoreName,
                    loc.City,
                    loc.Country,
                    loc.Latitude,
                    loc.Longitude,
                    loc.TopProduct,
                    loc.TopProductSales,
                    loc.SecondProduct,
                    loc.SecondProductSales,
                    loc.ThirdProduct,
                    loc.ThirdProductSales,
                    loc.TotalTransactions,
                    TopProductColor = ProductLocationDataLoader.GetProductColor(loc.TopProduct)
                }).ToList();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                string jsonData = JsonSerializer.Serialize(locationsWithColors, jsonOptions);
                await webViewProductMap.CoreWebView2.ExecuteScriptAsync($"updateProductMapData('{jsonData.Replace("'", "\\'")}')");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating product map: {ex.Message}");
            }
        }
    }
}