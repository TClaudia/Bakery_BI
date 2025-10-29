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
            // Validate input - Empty list is returned if there is no data or forecast months is invalid
            if (filteredData == null || !filteredData.Any() || forecastMonths <= 0)
                return new List<DataPoint>();

            // Time Normalization: Group by Month of TransactionDate and sum FinalAmount
            var monthlyData = filteredData
                // Group by month - normalized to the 1st of the month
                .GroupBy(x => new DateTime(x.TransactionDate.Year, x.TransactionDate.Month, 1))
                // Historical data is ordered chronologically
                .OrderBy(x => x.Key)
                // Total sales per month are calculated
                .Select((x, index) => new {
                    Date = x.Key,
                    Sales = x.Sum(x => x.FinalAmount), // Sum of all FinalAmount values per Month - represents the Y-value
                    Index = (double)index // double for regression calculation - represents the X-value
                })
                .ToList();

            // In case there are fewer than 2 points, regression cannot be calculated and only existing data is returned
            if (monthlyData.Count < 2)
                return monthlyData.Select(d => new DataPoint { Date = d.Date, Value = d.Sales }).ToList();

            // Linear Regression (y = mx + b)
            int N = monthlyData.Count;
            double sumX = monthlyData.Sum(d => d.Index);
            double sumY = (double)monthlyData.Sum(d => d.Sales);
            double sumX2 = monthlyData.Sum(d => d.Index * d.Index);
            double sumXY = monthlyData.Sum(d => d.Index * (double)d.Sales);

            // Calculate the denominator for the slope formula (m)
            double denominator = (N * sumX2 - sumX * sumX);

            // In case demonimator in 0 a trend cannot be calculated
            if (denominator == 0) return monthlyData.Select(d => new DataPoint { Date = d.Date, Value = d.Sales }).ToList();

            // Calculate the slope (m) by using the formula m = (N*Sum(XY) - Sum(X)*Sum(Y)) / (N*Sum(X^2) - Sum(X)^2)
            double m = (N * sumXY - sumX * sumY) / denominator;

            // Calculate the Y-intercept (b): b = (Sum(Y) - m*Sum(X)) / N
            double b = (sumY - m * sumX) / N;

            var trendAndForecastPoints = new List<DataPoint>();

            // Get the index and date of the last historical data point
            int lastIndex = (int)monthlyData.Last().Index;
            DateTime lastDate = monthlyData.Last().Date;

            // Use the calculated line equation (y = mx + b) to generate points for the historical period.
            foreach (var d in monthlyData)
                trendAndForecastPoints.Add(new DataPoint {
                    Date = d.Date,
                    Value = (decimal)(m * d.Index + b),
                    IsForecast = false // Mark as historical trend
                });

            // Loop from 1 up to the desired number of forecast months
            for (int i = 1; i <= forecastMonths; i++)
            {
                // Project the forecat date
                DateTime forecastDate = lastDate.AddMonths(i);
                // Project the index (X value) for the forecast period
                double forecastIndex = lastIndex + i;
                // Calculate the predicted sales (y value) using the line equation
                double forecastSalesDouble = m * forecastIndex + b;

                trendAndForecastPoints.Add(new DataPoint
                {
                    Date = forecastDate,
                    // Sales revenue should not be nagative
                    Value = (decimal)Math.Max(0, forecastSalesDouble),
                    IsForecast = true // Mark forecast data point
                });
            }

            //Return the combined list of historical and future forecast points
            return trendAndForecastPoints;
        }
    }
}
