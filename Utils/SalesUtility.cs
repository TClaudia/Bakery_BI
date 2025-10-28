using BakeryBI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBI.Utils
{
    internal class SalesUtility
    {
        public static List<SalesRecord> ApplyFilters(List<SalesRecord> rawData, List<string> selectedClientTypes)
        {
            if (rawData == null || selectedClientTypes == null || !selectedClientTypes.Any())
                return new List<SalesRecord>();

            return rawData
                .Where(x => selectedClientTypes.Contains(x.CustomerType)) // Uses CustomerType
                .ToList();
        }
        public static List<DataPoint> CalculateTrendAndForecast(List<SalesRecord> filteredData, int forecastMonths)
        {
            if (filteredData == null || !filteredData.Any() || forecastMonths <= 0)
                return new List<DataPoint>();

            // Time Normalization: Group by Month of TransactionDate and sum FinalAmount
            var monthlyData = filteredData
                .GroupBy(r => new DateTime(r.TransactionDate.Year, r.TransactionDate.Month, 1))
                .OrderBy(g => g.Key)
                .Select((g, index) => new {
                    Date = g.Key,
                    Sales = g.Sum(r => r.FinalAmount), // decimal
                    Index = (double)index // double for regression calculation
                })
                .ToList();

            if (monthlyData.Count < 2)
                return monthlyData.Select(d => new DataPoint { Date = d.Date, Value = d.Sales }).ToList();

            // Linear Regression (y = mx + b)
            int N = monthlyData.Count;
            double sumX = monthlyData.Sum(d => d.Index);
            double sumY = (double)monthlyData.Sum(d => d.Sales);
            double sumX2 = monthlyData.Sum(d => d.Index * d.Index);
            double sumXY = monthlyData.Sum(d => d.Index * (double)d.Sales);

            double denominator = (N * sumX2 - sumX * sumX);
            if (denominator == 0) return monthlyData.Select(d => new DataPoint { Date = d.Date, Value = d.Sales }).ToList();

            double m = (N * sumXY - sumX * sumY) / denominator; // Slope (double)
            double b = (sumY - m * sumX) / N; // Intercept (double)

            var trendAndForecastPoints = new List<DataPoint>();

            // Add trend line points and Forecast Points
            int lastIndex = (int)monthlyData.Last().Index;
            DateTime lastDate = monthlyData.Last().Date;

            foreach (var d in monthlyData) trendAndForecastPoints.Add(new DataPoint { Date = d.Date, Value = (decimal)(m * d.Index + b), IsForecast = false });

            for (int i = 1; i <= forecastMonths; i++)
            {
                DateTime forecastDate = lastDate.AddMonths(i);
                double forecastIndex = lastIndex + i;
                double forecastSalesDouble = m * forecastIndex + b;

                trendAndForecastPoints.Add(new DataPoint
                {
                    Date = forecastDate,
                    Value = (decimal)Math.Max(0, forecastSalesDouble),
                    IsForecast = true
                });
            }
            return trendAndForecastPoints;
        }
    }
}
