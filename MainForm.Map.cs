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
        private WebView2? webViewMap;
        private LocationDataLoader? locationLoader;
        private bool isMapInitialized = false;

        private void InitializeMapComponents()
        {
            locationLoader = new LocationDataLoader();

            webViewMap = new WebView2
            {
                Dock = DockStyle.Fill
            };

            var chart = chartSalesOverTime;
            var dataGrid = dgvSalesTimeData;

            splitContainerSales.Dock = DockStyle.Fill;

            splitContainerSales.Panel1.Controls.Clear();
            splitContainerSales.Panel2.Controls.Clear();

            // Set orientation to HORIZONTAL
            splitContainerSales.Orientation = Orientation.Horizontal;

            // We'll set this after the form loads, but provide a reasonable default
            int defaultHeight = splitContainerSales.Height > 0 ? splitContainerSales.Height : 800;
            splitContainerSales.SplitterDistance = defaultHeight / 2;

            splitContainerSales.IsSplitterFixed = false; 

            var splitTop = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 1000,
                IsSplitterFixed = false
            };

            // Add map to left panel
            webViewMap.Dock = DockStyle.Fill;
            splitTop.Panel1.Controls.Add(webViewMap);

            // Add data grid to right panel
            dataGrid.Dock = DockStyle.Fill;
            splitTop.Panel2.Controls.Add(dataGrid);

            // Add the top split to Panel1
            splitContainerSales.Panel1.Controls.Add(splitTop);

            
            chart.Dock = DockStyle.Fill;
            splitContainerSales.Panel2.Controls.Add(chart);

            splitContainerSales.SizeChanged += (s, e) =>
            {
                if (splitContainerSales.Height > 0)
                {
                    splitContainerSales.SplitterDistance = splitContainerSales.Height / 2;
                }
            };

            InitializeMapAsync();
        }

        private async void InitializeMapAsync()
        {
            if (webViewMap == null) return;

            try
            {
                await webViewMap.EnsureCoreWebView2Async(null);

                string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "map.html");

                if (allSalesData != null && allSalesData.Any())
                {
                    var allLocations = locationLoader?.GetStoreLocations(allSalesData);
                    if (allLocations != null && allLocations.Any())
                    {
                        decimal minSales = allLocations.Min(s => s.TotalSales);
                        decimal maxSales = allLocations.Max(s => s.TotalSales);

                        string dynamicHtml = GenerateDynamicMapHtml(minSales, maxSales);

                        string? resourcesDir = Path.GetDirectoryName(htmlPath);
                        if (resourcesDir != null && !Directory.Exists(resourcesDir))
                        {
                            Directory.CreateDirectory(resourcesDir);
                        }

                        File.WriteAllText(htmlPath, dynamicHtml);
                    }
                }
                else if (!File.Exists(htmlPath))
                {
                    CreateMapHtmlFile(htmlPath);
                }

                webViewMap.CoreWebView2.Navigate(new Uri(htmlPath).AbsoluteUri);

                webViewMap.CoreWebView2.NavigationCompleted += async (s, e) =>
                {
                    isMapInitialized = true;

                    // Force map resize
                    await Task.Delay(100);
                    await webViewMap.CoreWebView2.ExecuteScriptAsync("map.invalidateSize();");

                    UpdateMapWithCurrentData();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing map: {ex.Message}\n\nPlease ensure WebView2 Runtime is installed.",
                    "Map Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CreateMapHtmlFile(string htmlPath)
        {
            string? resourcesDir = Path.GetDirectoryName(htmlPath);
            if (resourcesDir != null && !Directory.Exists(resourcesDir))
            {
                Directory.CreateDirectory(resourcesDir);
            }

            string htmlContent = GenerateDynamicMapHtml(1000, 100000);

            File.WriteAllText(htmlPath, htmlContent);
        }

        private async void UpdateMapWithCurrentData()
        {
            if (!isMapInitialized || webViewMap == null || locationLoader == null || filteredData == null || !filteredData.Any())
                return;

            try
            {
                var storeLocations = locationLoader.GetStoreLocations(filteredData);

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                string jsonData = JsonSerializer.Serialize(storeLocations, jsonOptions);
                await webViewMap.CoreWebView2.ExecuteScriptAsync($"updateMapData('{jsonData.Replace("'", "\\'")}')");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating map: {ex.Message}");
            }
        }

        private string GenerateDynamicMapHtml(decimal minSales, decimal maxSales)
        {
            // Calculate dynamic intervals based on real data
            decimal range = maxSales - minSales;
            decimal step = range / 7; // 7 intervals for 8 colors

            var intervals = new[]
            {
                minSales + step * 7,
                minSales + step * 6,
                minSales + step * 5,
                minSales + step * 4,
                minSales + step * 3,
                minSales + step * 2,
                minSales + step * 1,
                minSales
            };

            string html = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'/>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Bakery Sales Map</title>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
    <style>
        body {{ margin: 0; padding: 0; font-family: Arial, sans-serif; }}
        #map {{ position: absolute; top: 0; bottom: 0; width: 100%; height: 100%; }}
        .legend {{ 
            background: white; 
            padding: 10px; 
            border-radius: 5px; 
            box-shadow: 0 0 15px rgba(0,0,0,0.2); 
            line-height: 24px; 
            color: #555; 
        }}
        .legend i {{ 
            width: 18px; 
            height: 18px; 
            float: left; 
            margin-right: 8px; 
            opacity: 0.7; 
            border-radius: 50%; 
        }}
        .store-popup {{ text-align: center; }}
        .store-popup h3 {{ margin: 0 0 10px 0; color: #2c3e50; }}
        .store-popup p {{ margin: 5px 0; }}
    </style>
</head>
<body>
    <div id='map'></div>
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <script>
       var map = L.map('map').setView([45.9432, 24.9668], 7.5);
        L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
            attribution: '© OpenStreetMap contributors',
            maxZoom: 18
        }}).addTo(map);
        
        var markersLayer = L.layerGroup().addTo(map);
        
        function getColor(sales) {{
            return sales > {intervals[0]:F2} ? '#800026' :
                   sales > {intervals[1]:F2} ? '#BD0026' :
                   sales > {intervals[2]:F2} ? '#E31A1C' :
                   sales > {intervals[3]:F2} ? '#FC4E2A' :
                   sales > {intervals[4]:F2} ? '#FD8D3C' :
                   sales > {intervals[5]:F2} ? '#FEB24C' :
                   sales > {intervals[6]:F2} ? '#FED976' : '#FFEDA0';
        }}
        
        function getRadius(sales) {{
            var minRadius = 15;
            var maxRadius = 40;
            var minSales = {minSales:F2};
            var maxSales = {maxSales:F2};
            var normalized = (sales - minSales) / (maxSales - minSales);
            return minRadius + (normalized * (maxRadius - minRadius));
        }}
        
        function updateMap(storesData) {{
            markersLayer.clearLayers();
            storesData.forEach(function(store) {{
                var radius = getRadius(store.totalSales);
                var color = getColor(store.totalSales);
                var circle = L.circleMarker([store.latitude, store.longitude], {{
                    radius: radius,
                    fillColor: color,
                    color: '#000',
                    weight: 2,
                    opacity: 1,
                    fillOpacity: 0.7
                }});
                
                var popupContent = '<div class=""store-popup"">' +
                    '<h3>' + store.storeName + '</h3>' +
                    '<p><strong>City:</strong> ' + store.city + '</p>' +
                    '<p><strong>Total Sales:</strong> $' + store.totalSales.toFixed(2).replace(/\d(?=(\d{{3}})+\.)/g, '$&,') + '</p>' +
                    '<p><strong>Transactions:</strong> ' + store.transactionCount.toString().replace(/\B(?=(\d{{3}})+(?!\d))/g, ',') + '</p>' +
                    '</div>';
                
                circle.bindPopup(popupContent);
                circle.addTo(markersLayer);
            }});
            
            if (storesData.length > 0) {{
                var group = new L.featureGroup(markersLayer.getLayers());
                map.fitBounds(group.getBounds().pad(0.1));
            }}
        }}
        
        var legend = L.control({{position: 'bottomright'}});
        legend.onAdd = function (map) {{
            var div = L.DomUtil.create('div', 'legend');
            div.innerHTML = '<h4 style=""margin: 0 0 10px 0;"">Sales Volume</h4>';
            var grades = [{intervals[7]:F0}, {intervals[6]:F0}, {intervals[5]:F0}, {intervals[4]:F0}, {intervals[3]:F0}, {intervals[2]:F0}, {intervals[1]:F0}, {intervals[0]:F0}];
            for (var i = 0; i < grades.length; i++) {{
                div.innerHTML +=
                    '<i style=""background:' + getColor(grades[i] + 1) + '""></i> ' +
                    '$' + grades[i].toString().replace(/\B(?=(\d{{3}})+(?!\d))/g, ',') + 
                    (grades[i + 1] ? '&ndash;$' + grades[i + 1].toString().replace(/\B(?=(\d{{3}})+(?!\d))/g, ',') + '<br>' : '+');
            }}
            return div;
        }};
        legend.addTo(map);
        
        window.updateMapData = function(jsonData) {{
            try {{
                var data = JSON.parse(jsonData);
                updateMap(data);
            }} catch (e) {{
                console.error('Error parsing map data:', e);
            }}
        }};
    </script>
</body>
</html>";

            return html;
        }
    }
}