using System;
using System.Collections.Generic;
using System.Linq;

namespace BakeryBI.Utils
{
    public class ForecastCalculator
    {
        public class ForecastResult
        {
            public decimal ForecastedValue { get; set; }
            public decimal Slope { get; set; }
            public string TrendDirection { get; set; }
        }

        public ForecastResult CalculateLinearForecast(List<decimal> values)
        {
            if (values == null || values.Count < 2)
            {
                return new ForecastResult
                {
                    ForecastedValue = 0,
                    Slope = 0,
                    TrendDirection = "Insufficient data"
                };
            }

            double n = values.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < values.Count; i++)
            {
                double x = i + 1;
                double y = (double)values[i];
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double intercept = (sumY - slope * sumX) / n;

            // Forecast next period
            double nextPeriod = n + 1;
            double forecast = slope * nextPeriod + intercept;

            string trend = slope > 0 ? "INCREASING ↗" : slope < 0 ? "DECREASING ↘" : "STABLE →";

            return new ForecastResult
            {
                ForecastedValue = (decimal)forecast,
                Slope = (decimal)slope,
                TrendDirection = trend
            };
        }
    }
}