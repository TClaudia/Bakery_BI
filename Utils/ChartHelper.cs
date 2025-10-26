using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace BakeryBI.Utils
{
    public static class ChartHelper
    {
        public static void SetupBarChart(Chart chart, string title)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Legends.Clear();

            // Chart Area
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Product";
            chartArea.AxisY.Title = "Amount ($)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.BackColor = Color.WhiteSmoke;
            chart.ChartAreas.Add(chartArea);

            // Sales Series
            Series salesSeries = new Series("Sales")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(46, 139, 87),
                BorderWidth = 1,
                BorderColor = Color.DarkGreen
            };
            chart.Series.Add(salesSeries);

            // Costs Series
            Series costsSeries = new Series("Costs")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(220, 20, 60),
                BorderWidth = 1,
                BorderColor = Color.DarkRed
            };
            chart.Series.Add(costsSeries);

            // Legend
            Legend legend = new Legend("Legend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center
            };
            chart.Legends.Add(legend);

            // Title
            chart.Titles.Clear();
            chart.Titles.Add(new Title(title, Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.Black));
        }

        public static void SetupLineChart(Chart chart, string title)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Legends.Clear();

            // Chart Area
            ChartArea chartArea = new ChartArea("ProfitArea");
            chartArea.AxisX.Title = "Period";
            chartArea.AxisY.Title = "Profit ($)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.BackColor = Color.AliceBlue;
            chart.ChartAreas.Add(chartArea);

            // Profit Series
            Series profitSeries = new Series("Profit")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                MarkerColor = Color.DarkBlue
            };
            chart.Series.Add(profitSeries);

            // Legend
            Legend legend = new Legend("Legend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center
            };
            chart.Legends.Add(legend);

            // Title
            chart.Titles.Clear();
            chart.Titles.Add(new Title(title, Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.Black));
        }
    }
}