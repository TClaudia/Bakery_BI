using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BakeryBI.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace BakeryBI.Utils
{
    public static class ExcelExporter
    {
        /// <summary>
        /// Applies icon set conditional formatting to a column in an Excel worksheet
        /// </summary>
        /// <param name="worksheet">The Excel worksheet to apply formatting to</param>
        /// <param name="columnLetter">The column letter (e.g., "B", "C") to apply formatting to</param>
        /// <param name="lastDataRow">The last row number containing data (1-based)</param>
        /// <param name="startConfigRow">The starting row number for configuration cells (1-based)</param>
        private static void ApplyIconSetConditionalFormatting(Excel.Worksheet worksheet, string columnLetter, int lastDataRow, int startConfigRow)
        {
            if (lastDataRow < 2)
                return;

            int configRow = startConfigRow;
            
            // Header for threshold configuration
            Excel.Range thresholdHeader = worksheet.Cells[configRow, 1];
            thresholdHeader.Value2 = "Icon Set Thresholds (Percentiles)";
            thresholdHeader.Font.Bold = true;
            thresholdHeader.Font.Size = 11;

            // Low threshold cell
            Excel.Range lowThresholdLabel = worksheet.Cells[++configRow, 1];
            Excel.Range lowThresholdCell = worksheet.Cells[configRow, 2];
            lowThresholdLabel.Value2 = "Low Threshold (%):";
            lowThresholdCell.Value2 = 33;
            lowThresholdCell.NumberFormat = "0";
            lowThresholdCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
            string lowThresholdRef = lowThresholdCell.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);

            // High threshold cell
            Excel.Range highThresholdLabel = worksheet.Cells[++configRow, 1];
            Excel.Range highThresholdCell = worksheet.Cells[configRow, 2];
            highThresholdLabel.Value2 = "High Threshold (%):";
            highThresholdCell.Value2 = 67;
            highThresholdCell.NumberFormat = "0";
            highThresholdCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
            string highThresholdRef = highThresholdCell.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);

            // Helper formula cells
            configRow++;
            Excel.Range lowValueLabel = worksheet.Cells[configRow, 1];
            Excel.Range lowValueCell = worksheet.Cells[configRow, 2];
            lowValueLabel.Value2 = "Low Threshold Value:";
            lowValueCell.Formula = $"=PERCENTILE(${columnLetter}$2:${columnLetter}${lastDataRow},{lowThresholdRef}/100)";
            lowValueCell.NumberFormat = "$#,##0.00";
            string lowValueRef = lowValueCell.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);

            configRow++;
            Excel.Range highValueLabel = worksheet.Cells[configRow, 1];
            Excel.Range highValueCell = worksheet.Cells[configRow, 2];
            highValueLabel.Value2 = "High Threshold Value:";
            highValueCell.Formula = $"=PERCENTILE(${columnLetter}$2:${columnLetter}${lastDataRow},{highThresholdRef}/100)";
            highValueCell.NumberFormat = "$#,##0.00";
            string highValueRef = highValueCell.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);

            // Apply icon set conditional formatting to the specified column
            Excel.Range dataRange = worksheet.Range[$"{columnLetter}2:{columnLetter}{lastDataRow}"];

            object iconSetObj = dataRange.FormatConditions.AddIconSetCondition();

            // Try to cast, but if it fails, we'll work with the object directly using reflection
            Excel.FormatCondition iconSet = null;
            try
            {
                iconSet = iconSetObj as Excel.FormatCondition;
                if (iconSet == null && iconSetObj != null)
                {
                    iconSet = (Excel.FormatCondition)iconSetObj;
                }
            }
            catch
            {
                iconSet = null;
            }

            object formatConditionObj = iconSet != null ? (object)iconSet : iconSetObj;

            try
            {
                System.Reflection.PropertyInfo iconSetsProp = worksheet.Application.GetType().GetProperty("IconSets");
                if (iconSetsProp != null)
                {
                    object iconSetsCollection = iconSetsProp.GetValue(worksheet.Application);
                    if (iconSetsCollection != null)
                    {
                        // Get the 3 Traffic Lights icon set from the collection
                        System.Reflection.PropertyInfo indexer = iconSetsCollection.GetType().GetProperty("Item");
                        if (indexer != null)
                        {
                            object trafficLightsIconSet = indexer.GetValue(iconSetsCollection, new object[] { Excel.XlIconSet.xl3TrafficLights1 });

                            // Set IconSet property using reflection
                            System.Reflection.PropertyInfo iconSetProp = formatConditionObj.GetType().GetProperty("IconSet");
                            if (iconSetProp != null && iconSetProp.CanWrite)
                            {
                                iconSetProp.SetValue(formatConditionObj, trafficLightsIconSet, null);
                            }
                        }
                    }
                }
            }
            catch
            {
                // If setting IconSet fails, Excel will use default icon set
                // Users can manually change it in Excel if needed
            }

            try
            {
                // Get IconCriteria property
                System.Reflection.PropertyInfo iconCriteriaProp = formatConditionObj.GetType().GetProperty("IconCriteria");
                if (iconCriteriaProp != null)
                {
                    object criteriaObj = iconCriteriaProp.GetValue(formatConditionObj);
                    if (criteriaObj != null)
                    {
                        // Get the calculated values from helper cells
                        double lowThresholdValue = (double)lowValueCell.Value2;
                        double highThresholdValue = (double)highValueCell.Value2;

                        // Get Item method to access individual criteria
                        System.Reflection.MethodInfo itemMethod = criteriaObj.GetType().GetMethod("Item", new Type[] { typeof(int) });
                        if (itemMethod != null)
                        {
                            // Icon 1 (Red): Values <= Low threshold value
                            object item1 = itemMethod.Invoke(criteriaObj, new object[] { 1 });
                            if (item1 != null)
                            {
                                System.Reflection.PropertyInfo type1 = item1.GetType().GetProperty("Type");
                                System.Reflection.PropertyInfo value1 = item1.GetType().GetProperty("Value");
                                if (type1 != null) type1.SetValue(item1, Excel.XlConditionValueTypes.xlConditionValueNumber);
                                if (value1 != null) value1.SetValue(item1, lowThresholdValue);
                            }

                            // Icon 2 (Yellow): Values between Low and High threshold values
                            object item2 = itemMethod.Invoke(criteriaObj, new object[] { 2 });
                            if (item2 != null)
                            {
                                System.Reflection.PropertyInfo type2 = item2.GetType().GetProperty("Type");
                                System.Reflection.PropertyInfo value2 = item2.GetType().GetProperty("Value");
                                if (type2 != null) type2.SetValue(item2, Excel.XlConditionValueTypes.xlConditionValueNumber);
                                if (value2 != null) value2.SetValue(item2, highThresholdValue);
                            }

                            // Icon 3 (Green): Values >= High threshold value
                            object item3 = itemMethod.Invoke(criteriaObj, new object[] { 3 });
                            if (item3 != null)
                            {
                                System.Reflection.PropertyInfo type3 = item3.GetType().GetProperty("Type");
                                System.Reflection.PropertyInfo value3 = item3.GetType().GetProperty("Value");
                                if (type3 != null) type3.SetValue(item3, Excel.XlConditionValueTypes.xlConditionValueNumber);
                                if (value3 != null) value3.SetValue(item3, highThresholdValue);
                            }
                        }
                    }
                }
            }
            catch
            {
                // If setting criteria fails, Excel will use default criteria
                // Users can manually adjust in Excel if needed
            }

            // Add helpful note for users
            configRow++;
            Excel.Range noteCell = worksheet.Cells[configRow, 1];
            noteCell.Value2 = $"To enable auto-update: Edit CF rule and reference cells {lowValueRef} and {highValueRef}";
            noteCell.Font.Italic = true;
            noteCell.Font.Size = 9;
            noteCell.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Gray);
            noteCell.WrapText = true;
        }
        /// <summary>
        /// Exports future sales estimation data to Excel with descriptive and predictive analytics
        /// </summary>
        /// <param name="filePath">Path where the Excel file will be saved</param>
        /// <param name="filteredData">Filtered sales data to export</param>
        /// <param name="forecastMonths">Number of months to forecast</param>
        public static void ExportFutureSalesToExcel(string filePath, List<SalesRecord> filteredData, int forecastMonths)
        {
            if (filteredData == null || !filteredData.Any())
            {
                MessageBox.Show("No data available to export.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Add();

                // Sheet 1: Monthly Sales Data (Descriptive Analytics)
                Excel.Worksheet monthlySheet = (Excel.Worksheet)workbook.Worksheets[1];
                monthlySheet.Name = "Monthly Sales Data";

                // Headers
                ((Excel.Range)monthlySheet.Cells[1, 1]).Value2 = "Month";
                ((Excel.Range)monthlySheet.Cells[1, 2]).Value2 = "Total Sales";
                ((Excel.Range)monthlySheet.Cells[1, 3]).Value2 = "Transaction Count";

                // Style headers
                Excel.Range headerRange1 = monthlySheet.Range["A1", "C1"];
                headerRange1.Font.Bold = true;
                headerRange1.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);

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
                    }).ToList();

                // Populate data
                int row = 2;
                foreach (var item in monthlySales)
                {
                    ((Excel.Range)monthlySheet.Cells[row, 1]).Value2 = item.Month.ToString("MMM yyyy");
                    ((Excel.Range)monthlySheet.Cells[row, 2]).Value2 = (double)item.TotalSales;
                    ((Excel.Range)monthlySheet.Cells[row, 2]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)monthlySheet.Cells[row, 3]).Value2 = item.TransactionCount;
                    row++;
                }

                // Add summary statistics (Descriptive Analytics)
                int summaryRow = row + 2;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "SUMMARY STATISTICS";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Font.Bold = true;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Font.Size = 12;
                summaryRow++;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Total Sales:";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Sum(x => x.TotalSales);
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Font.Bold = true;
                summaryRow++;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Average Monthly Sales:";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Average(x => x.TotalSales);
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                summaryRow++;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Maximum Monthly Sales:";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Max(x => x.TotalSales);
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                summaryRow++;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Minimum Monthly Sales:";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = (double)monthlySales.Min(x => x.TotalSales);
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                summaryRow++;
                ((Excel.Range)monthlySheet.Cells[summaryRow, 1]).Value2 = "Total Transactions:";
                ((Excel.Range)monthlySheet.Cells[summaryRow, 2]).Value2 = monthlySales.Sum(x => x.TransactionCount);

                // Apply icon set conditional formatting to Total Sales column (Column B)
                int lastDataRow = row - 1;
                ApplyIconSetConditionalFormatting(monthlySheet, "B", lastDataRow, summaryRow + 3);

                // Auto-fit columns
                monthlySheet.Columns.AutoFit();

                // Sheet 2: Forecast Data (Predictive Analytics)
                Excel.Worksheet forecastSheet = (Excel.Worksheet)workbook.Worksheets.Add();
                forecastSheet.Name = "Forecast Data";

                // Headers
                ((Excel.Range)forecastSheet.Cells[1, 1]).Value2 = "Date";
                ((Excel.Range)forecastSheet.Cells[1, 2]).Value2 = "Type";
                ((Excel.Range)forecastSheet.Cells[1, 3]).Value2 = "Sales Forecast";

                // Style headers
                Excel.Range headerRange2 = forecastSheet.Range["A1", "C1"];
                headerRange2.Font.Bold = true;
                headerRange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGreen);

                // Get forecast data
                // NOTE: Uses filteredData which respects all global filters (Date Range, Store, Product)
                var trendAndForecast = SalesUtility.CalculateTrendAndForecast(filteredData, forecastMonths);

                // Populate forecast data
                row = 2;
                foreach (var point in trendAndForecast)
                {
                    ((Excel.Range)forecastSheet.Cells[row, 1]).Value2 = point.Date;
                    ((Excel.Range)forecastSheet.Cells[row, 1]).NumberFormat = "MMM yyyy";
                    ((Excel.Range)forecastSheet.Cells[row, 2]).Value2 = point.IsForecast ? "Forecast" : "Historical Trend";
                    ((Excel.Range)forecastSheet.Cells[row, 3]).Value2 = (double)point.Value;
                    ((Excel.Range)forecastSheet.Cells[row, 3]).NumberFormat = "$#,##0.00";

                    // Highlight forecast rows
                    if (point.IsForecast)
                    {
                        Excel.Range forecastRowRange = forecastSheet.Range[forecastSheet.Cells[row, 1], forecastSheet.Cells[row, 3]];
                        forecastRowRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                        ((Excel.Range)forecastSheet.Cells[row, 2]).Font.Italic = true;
                    }
                    row++;
                }

                // Add forecast summary
                var forecastPoints = trendAndForecast.Where(p => p.IsForecast).ToList();
                if (forecastPoints.Any())
                {
                    summaryRow = row + 2;
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "FORECAST SUMMARY";
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Font.Bold = true;
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Font.Size = 12;
                    summaryRow++;
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "Forecast Period:";
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Value2 = $"{forecastMonths} months";
                    summaryRow++;
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "Total Forecasted Sales:";
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Value2 = (double)forecastPoints.Sum(p => p.Value);
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Font.Bold = true;
                    summaryRow++;
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 1]).Value2 = "Average Monthly Forecast:";
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).Value2 = (double)forecastPoints.Average(p => p.Value);
                    ((Excel.Range)forecastSheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                }
                else
                {
                    // If no forecast points, set summaryRow to after the data
                    summaryRow = row + 2;
                }

                // Apply icon set conditional formatting to Sales Forecast column (Column C)
                int lastDataRow = row - 1;
                ApplyIconSetConditionalFormatting(forecastSheet, "C", lastDataRow, summaryRow + 3);

                // Auto-fit columns
                forecastSheet.Columns.AutoFit();

                // Sheet 3: Chart Sheet (Historical Data, Trend, and Forecast)
                // Create a temporary data sheet for chart data first
                Excel.Worksheet chartDataSheet = (Excel.Worksheet)workbook.Worksheets.Add();
                chartDataSheet.Name = "Chart Data";

                // Calculate monthly sales for Actual Sales column
                var monthlySalesSummary = filteredData
                    .GroupBy(x => new DateTime(x.TransactionDate.Year, x.TransactionDate.Month, 1))
                    .OrderBy(x => x.Key)
                    .Select(x => new
                    {
                        Month = x.Key,
                        TotalSales = x.Sum(r => r.FinalAmount)
                    })
                    .ToList();

                // Read data from Forecast Data sheet
                // Column A: Date, Column B: Type, Column C: Sales Forecast
                var forecastDataList = new List<(DateTime Date, string Type, double SalesForecast)>();

                int forecastDataRow = 2; // Start after header
                while (true)
                {
                    object dateObj = ((Excel.Range)forecastSheet.Cells[forecastDataRow, 1]).Value2;
                    object typeObj = ((Excel.Range)forecastSheet.Cells[forecastDataRow, 2]).Value2;
                    object salesObj = ((Excel.Range)forecastSheet.Cells[forecastDataRow, 3]).Value2;

                    // Stop if we hit an empty row
                    if (dateObj == null || typeObj == null || salesObj == null)
                        break;

                    DateTime date = DateTime.FromOADate((double)dateObj);
                    string type = typeObj.ToString();
                    double salesForecast = (double)salesObj;

                    forecastDataList.Add((date, type, salesForecast));
                    forecastDataRow++;
                }

                // Populate chart data: Month, Actual Sales, Actual Sales Line (for trendline), Forecast (Dots)
                int chartDataRow = 1;

                ((Excel.Range)chartDataSheet.Cells[chartDataRow, 1]).Value2 = "Month";
                ((Excel.Range)chartDataSheet.Cells[chartDataRow, 2]).Value2 = "Actual Sales";
                ((Excel.Range)chartDataSheet.Cells[chartDataRow, 3]).Value2 = "Actual Sales Line";
                ((Excel.Range)chartDataSheet.Cells[chartDataRow, 4]).Value2 = "Forecast";

                chartDataRow = 2;

                // Get all unique months from forecast data and actual sales
                var allMonths = forecastDataList.Select(f => new DateTime(f.Date.Year, f.Date.Month, 1))
                    .Union(monthlySalesSummary.Select(m => new DateTime(m.Month.Year, m.Month.Month, 1)))
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                // Find the last historical month (for forecast calculation)
                DateTime? lastHistoricalMonth = monthlySalesSummary.Any()
                    ? new DateTime(monthlySalesSummary.Max(m => m.Month).Year, monthlySalesSummary.Max(m => m.Month).Month, 1)
                    : null;

                // Determine the range of historical data for FORECAST.LINEAR formula and trendline
                // Calculate these during data population to get correct row numbers
                int firstHistoricalRow = 0;
                int lastHistoricalRow = 0;

                foreach (var month in allMonths)
                {
                    var normalizedMonth = new DateTime(month.Year, month.Month, 1);

                    ((Excel.Range)chartDataSheet.Cells[chartDataRow, 1]).Value2 = normalizedMonth;
                    ((Excel.Range)chartDataSheet.Cells[chartDataRow, 1]).NumberFormat = "MMM yyyy";

                    // Actual Sales (only for months with actual sales data)
                    var actualSales = monthlySalesSummary.FirstOrDefault(m =>
                        new DateTime(m.Month.Year, m.Month.Month, 1) == normalizedMonth);
                    if (actualSales != null)
                    {
                        ((Excel.Range)chartDataSheet.Cells[chartDataRow, 2]).Value2 = (double)actualSales.TotalSales;

                        // Track historical data rows for trendline calculation
                        if (firstHistoricalRow == 0) firstHistoricalRow = chartDataRow;
                        lastHistoricalRow = chartDataRow;
                    }
                    else
                    {
                        ((Excel.Range)chartDataSheet.Cells[chartDataRow, 2]).Value2 = "";
                    }

                    // Column 3: Actual Sales Line (same as Column 2, but for line series - used for trendline calculation)
                    // This will be invisible but needed for Excel to calculate the trendline
                    // IMPORTANT: Only populate for historical months (not forecast months)
                    if (actualSales != null)
                    {
                        ((Excel.Range)chartDataSheet.Cells[chartDataRow, 3]).Value2 = (double)actualSales.TotalSales;
                    }
                    else
                    {
                        // Leave empty for forecast months - this ensures trendline only uses historical data
                        ((Excel.Range)chartDataSheet.Cells[chartDataRow, 3]).Value2 = "";
                    }

                    // Column 4: Forecast - use Excel's FORECAST.LINEAR function for forecast months
                    // Only calculate forecast for months after the last historical month
                    bool isForecastMonth = lastHistoricalMonth.HasValue && normalizedMonth > lastHistoricalMonth.Value;

                    if (isForecastMonth && firstHistoricalRow > 0 && lastHistoricalRow > 0)
                    {
                        // Use Excel's FORECAST.LINEAR function
                        // FORECAST.LINEAR(x, known_y's, known_x's)
                        // x = current month's index (1, 2, 3, ... based on position)
                        // known_y's = historical sales values (Column 2)
                        // known_x's = historical indices (1, 2, 3, ... for each historical month)

                        // Calculate the index for this forecast month (1-based, relative to first historical month)
                        int forecastIndex = chartDataRow - firstHistoricalRow + 1;

                        // Build the formula with absolute references
                        string knownYsRange = $"$B${firstHistoricalRow}:$B${lastHistoricalRow}";
                        string knownXsRange = $"ROW($A${firstHistoricalRow}:$A${lastHistoricalRow})-ROW($A${firstHistoricalRow})+1";
                        string xValue = forecastIndex.ToString();

                        // Use FORECAST.LINEAR (Excel 2016+) - calculates linear forecast based on historical data
                        // This is Excel's native function, similar to how trendlines work
                        string forecastFormula = $"=FORECAST.LINEAR({xValue},{knownYsRange},{knownXsRange})";

                        ((Excel.Range)chartDataSheet.Cells[chartDataRow, 4]).Formula = forecastFormula;
                    }
                    else
                    {
                        ((Excel.Range)chartDataSheet.Cells[chartDataRow, 4]).Value2 = "";
                    }

                    chartDataRow++;
                }

                // Create chart from the data sheet
                int lastRow = chartDataRow - 1;
                // Chart range includes: Month, Actual Sales, Trend & Forecast (Historical), Forecast (Dots)
                Excel.Range chartRange = chartDataSheet.Range[chartDataSheet.Cells[1, 1], chartDataSheet.Cells[lastRow, 4]];

                // Get ChartObjects - with embedded interop types, this might return object
                object chartObjectsObj = chartDataSheet.ChartObjects();
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)chartObjectsObj;
                Excel.ChartObject chartObject = (Excel.ChartObject)chartObjects.Add(0, 0, 600, 400);
                Excel.Chart chartSheet = chartObject.Chart;

                // Set chart data source
                chartSheet.SetSourceData(chartRange);

                // Configure chart to treat empty cells as gaps (not zeros) - must be set before adding series
                chartSheet.DisplayBlanksAs = Excel.XlDisplayBlanksAs.xlNotPlotted;

                // Configure chart type - Combo chart (Column + Line)
                chartSheet.ChartType = Excel.XlChartType.xlColumnClustered;

                // Get chart series collection
                Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chartSheet.SeriesCollection();

                // Series 1: Actual Sales (Column chart)
                if (seriesCollection.Count >= 1)
                {
                    Excel.Series actualSeries = (Excel.Series)seriesCollection.Item(1);
                    actualSeries.Name = "Actual Sales";
                    actualSeries.ChartType = Excel.XlChartType.xlColumnClustered;
                    actualSeries.Format.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);
                }

                // Series 2: Actual Sales Line (for trendline calculation)
                // Create a line series with actual sales data - trendlines work on line/scatter charts
                // IMPORTANT: Limit the series to only historical data rows to prevent trendline extension
                if (seriesCollection.Count >= 2 && firstHistoricalRow > 0 && lastHistoricalRow > 0)
                {
                    Excel.Series actualSalesLineSeries = (Excel.Series)seriesCollection.Item(2);
                    actualSalesLineSeries.Name = "Actual Sales Line";
                    actualSalesLineSeries.ChartType = Excel.XlChartType.xlLine;

                    // Set the series to only use historical data (not forecast months)
                    // This ensures the trendline only calculates from historical data
                    Excel.Range historicalValuesRange = chartDataSheet.Range[
                        chartDataSheet.Cells[firstHistoricalRow, 3],
                        chartDataSheet.Cells[lastHistoricalRow, 3]];
                    Excel.Range historicalXValuesRange = chartDataSheet.Range[
                        chartDataSheet.Cells[firstHistoricalRow, 1],
                        chartDataSheet.Cells[lastHistoricalRow, 1]];

                    actualSalesLineSeries.Values = historicalValuesRange;
                    actualSalesLineSeries.XValues = historicalXValuesRange;

                    // Make this series invisible (we only need it for the trendline)
                    actualSalesLineSeries.Format.Line.Visible = 0; // Hide the line
                    actualSalesLineSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone; // No markers

                    // Add linear trendline to this series - only for historical data (no forward extension)
                    Excel.Trendlines trendlines = (Excel.Trendlines)actualSalesLineSeries.Trendlines();
                    Excel.Trendline trendline = (Excel.Trendline)trendlines.Add(Excel.XlTrendlineType.xlLinear);

                    // Configure trendline - only shows historical trend, not forecast
                    trendline.Name = "Trend";
                    trendline.Format.Line.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(Color.Red);
                    trendline.Format.Line.Weight = 3;

                    // Do NOT extend trendline forward - only show historical trend
                    trendline.Forward = 0;

                    // Optional: Display equation and R-squared on chart
                    trendline.DisplayEquation = false;
                    trendline.DisplayRSquared = false;
                }

                // Series 3: Forecast (Dots only - from Forecast Data sheet, Type = "Forecast")
                // Data is already populated in column 4 from Forecast Data sheet
                if (seriesCollection.Count >= 3)
                {
                    Excel.Series forecastSeries = (Excel.Series)seriesCollection.Item(3);
                    forecastSeries.Name = "Forecast";
                    forecastSeries.ChartType = Excel.XlChartType.xlLine;

                    // Hide the line - show only dots (markers) for forecast points
                    forecastSeries.Format.Line.Visible = 0; // 0 = msoFalse (invisible line)

                    // Configure markers (dots) only
                    forecastSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                    forecastSeries.MarkerSize = 8; // Larger dots for better visibility

                    // Set marker color to red
                    forecastSeries.MarkerForegroundColor = System.Drawing.ColorTranslator.ToOle(Color.Red);
                    forecastSeries.MarkerBackgroundColor = System.Drawing.ColorTranslator.ToOle(Color.Red);
                }
                else if (forecastDataList.Any(f => f.Type == "Forecast"))
                {
                    // Add forecast series if it doesn't exist yet
                    Excel.Range forecastRange = chartDataSheet.Range[chartDataSheet.Cells[1, 4], chartDataSheet.Cells[lastRow, 4]];
                    Excel.Series forecastSeries = (Excel.Series)seriesCollection.NewSeries();

                    forecastSeries.Name = "Forecast";
                    forecastSeries.ChartType = Excel.XlChartType.xlLine;
                    forecastSeries.Values = forecastRange;
                    forecastSeries.XValues = chartDataSheet.Range[chartDataSheet.Cells[2, 1], chartDataSheet.Cells[lastRow, 1]];

                    // Hide the line - show only dots (markers) for forecast points
                    forecastSeries.Format.Line.Visible = 0; // 0 = msoFalse (invisible line)

                    // Configure markers (dots) only
                    forecastSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                    forecastSeries.MarkerSize = 8; // Larger dots for better visibility

                    // Set marker color to red
                    forecastSeries.MarkerForegroundColor = System.Drawing.ColorTranslator.ToOle(Color.Red);
                    forecastSeries.MarkerBackgroundColor = System.Drawing.ColorTranslator.ToOle(Color.Red);
                }

                // Chart title
                chartSheet.HasTitle = true;
                chartSheet.ChartTitle.Text = "Future Sales Estimation (Monthly Revenue Trend)";
                chartSheet.ChartTitle.Font.Size = 14;
                chartSheet.ChartTitle.Font.Bold = true;

                // Axis titles
                Excel.Axis categoryAxis = (Excel.Axis)chartSheet.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                categoryAxis.HasTitle = true;
                categoryAxis.AxisTitle.Text = "Month";
                categoryAxis.AxisTitle.Font.Size = 11;
                categoryAxis.AxisTitle.Font.Bold = true;
                Excel.Axis valueAxis = (Excel.Axis)chartSheet.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                valueAxis.HasTitle = true;
                valueAxis.AxisTitle.Text = "Total Sales (Revenue)";
                valueAxis.AxisTitle.Font.Size = 11;
                valueAxis.AxisTitle.Font.Bold = true;
                // Format value axis as currency
                valueAxis.TickLabels.NumberFormat = "$#,##0";
                // Format category axis dates
                categoryAxis.CategoryType = Excel.XlCategoryType.xlCategoryScale;
                categoryAxis.TickLabels.NumberFormat = "MMM yy";
                // Legend
                chartSheet.HasLegend = true;
                chartSheet.Legend.Position = Excel.XlLegendPosition.xlLegendPositionTop;
                // Chart area formatting
                chartSheet.PlotArea.Format.Fill.Visible = 0; // 0 = msoFalse
                chartSheet.ChartArea.Format.Fill.Visible = 0; // 0 = msoFalse
                chartSheet.Location(Excel.XlChartLocation.xlLocationAsNewSheet, "Sales Trend Chart");
                // Hide the temporary data sheet
                chartDataSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;

                // Save file
                workbook.SaveAs(filePath);
            }
            finally
            {
                // Clean up COM objects
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        /// <summary>
        /// Exports profit evolution data to Excel with descriptive analytics
        /// </summary>
        /// <param name="filePath">Path where the Excel file will be saved</param>
        /// <param name="filteredData">Filtered sales data to export</param>
        /// <param name="selectedClientTypes">List of selected client types for filtering</param>
        /// <param name="selectedStoreNames">List of selected store names for filtering</param>
        public static void ExportProfitsToExcel(string filePath, List<SalesRecord> filteredData, 
            List<string> selectedClientTypes, List<string> selectedStoreNames)
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
                MessageBox.Show("No data available for selected filters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;
                workbook = excelApp.Workbooks.Add();

                // Sheet 1: Monthly Profit by Store (Descriptive Analytics)
                Excel.Worksheet profitSheet = (Excel.Worksheet)workbook.Worksheets[1];
                profitSheet.Name = "Monthly Profit by Store";

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
                    }).ToList();

                // Get unique stores and months for pivot structure
                var stores = monthlyProfit.Select(x => x.Store).Distinct().OrderBy(s => s).ToList();
                var months = monthlyProfit.Select(x => x.Month).Distinct().OrderBy(m => m).ToList();

                // Headers - First column is Month, then one column per store
                ((Excel.Range)profitSheet.Cells[1, 1]).Value2 = "Month";
                int col = 2;
                foreach (var store in stores)
                {
                    ((Excel.Range)profitSheet.Cells[1, col]).Value2 = store;
                    col++;
                }
                ((Excel.Range)profitSheet.Cells[1, col]).Value2 = "Total";

                // Style headers
                Excel.Range headerRange4 = profitSheet.Range[profitSheet.Cells[1, 1], profitSheet.Cells[1, col]];
                headerRange4.Font.Bold = true;
                headerRange4.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);

                // Populate data
                int row = 2;
                foreach (var month in months)
                {
                    ((Excel.Range)profitSheet.Cells[row, 1]).Value2 = month.ToString("MMM yyyy");
                    ((Excel.Range)profitSheet.Cells[row, 1]).NumberFormat = "MMM yyyy";
                    col = 2;
                    decimal monthTotal = 0;
                    foreach (var store in stores)
                    {
                        var storeData = monthlyProfit.FirstOrDefault(x => x.Month == month && x.Store == store);
                        decimal profit = storeData?.Profit ?? 0;
                        ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)profit;
                        ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";
                        monthTotal += profit;
                        col++;
                    }
                    // Total for the month
                    ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)monthTotal;
                    ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)profitSheet.Cells[row, col]).Font.Bold = true;
                    row++;
                }
                // Add totals row
                ((Excel.Range)profitSheet.Cells[row, 1]).Value2 = "TOTAL";
                ((Excel.Range)profitSheet.Cells[row, 1]).Font.Bold = true;
                col = 2;
                foreach (var store in stores)
                {
                    decimal storeTotal = monthlyProfit.Where(x => x.Store == store).Sum(x => x.Profit);
                    ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)storeTotal;
                    ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)profitSheet.Cells[row, col]).Font.Bold = true;
                    col++;
                }
                decimal grandTotal = monthlyProfit.Sum(x => x.Profit);
                ((Excel.Range)profitSheet.Cells[row, col]).Value2 = (double)grandTotal;
                ((Excel.Range)profitSheet.Cells[row, col]).NumberFormat = "$#,##0.00";
                ((Excel.Range)profitSheet.Cells[row, col]).Font.Bold = true;
                // Auto-fit columns
                profitSheet.Columns.AutoFit();
                // Sheet 2: Store Performance Summary (Descriptive Analytics)
                Excel.Worksheet summarySheet = (Excel.Worksheet)workbook.Worksheets.Add();
                summarySheet.Name = "Store Performance Summary";
                // Headers
                ((Excel.Range)summarySheet.Cells[1, 1]).Value2 = "Store";
                ((Excel.Range)summarySheet.Cells[1, 2]).Value2 = "Total Profit";
                ((Excel.Range)summarySheet.Cells[1, 3]).Value2 = "Total Sales";
                ((Excel.Range)summarySheet.Cells[1, 4]).Value2 = "Profit Margin %";
                ((Excel.Range)summarySheet.Cells[1, 5]).Value2 = "Avg Monthly Profit";
                ((Excel.Range)summarySheet.Cells[1, 6]).Value2 = "Transaction Count";
                // Style headers
                Excel.Range headerRange5 = summarySheet.Range["A1", "F1"];
                headerRange5.Font.Bold = true;
                headerRange5.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGreen);
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
                    .OrderByDescending(x => x.TotalProfit).ToList();
                // Populate summary data
                row = 2;
                foreach (var summary in storeSummaries)
                {
                    ((Excel.Range)summarySheet.Cells[row, 1]).Value2 = summary.Store;
                    ((Excel.Range)summarySheet.Cells[row, 2]).Value2 = (double)summary.TotalProfit;
                    ((Excel.Range)summarySheet.Cells[row, 2]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)summarySheet.Cells[row, 3]).Value2 = (double)summary.TotalSales;
                    ((Excel.Range)summarySheet.Cells[row, 3]).NumberFormat = "$#,##0.00";
                    decimal profitMargin = summary.TotalSales != 0
                    ? (summary.TotalProfit / summary.TotalSales) * 100 : 0;
                    ((Excel.Range)summarySheet.Cells[row, 4]).Value2 = (double)profitMargin;
                    ((Excel.Range)summarySheet.Cells[row, 4]).NumberFormat = "0.00%";
                    ((Excel.Range)summarySheet.Cells[row, 5]).Value2 = (double)summary.AvgMonthlyProfit;
                    ((Excel.Range)summarySheet.Cells[row, 5]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)summarySheet.Cells[row, 6]).Value2 = summary.TransactionCount;
                    row++;
                }
                // Add overall summary
                int summaryRow = row + 2;
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "OVERALL SUMMARY";
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Font.Bold = true;
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Font.Size = 12;
                summaryRow++;
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Total Profit (All Stores):";
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = (double)storeSummaries.Sum(x => x.TotalProfit);
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Font.Bold = true;
                summaryRow++;
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Total Sales (All Stores):";
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = (double)storeSummaries.Sum(x => x.TotalSales);
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).NumberFormat = "$#,##0.00";
                summaryRow++;
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Overall Profit Margin:";
                decimal overallMargin = storeSummaries.Sum(x => x.TotalSales) != 0
                ? (storeSummaries.Sum(x => x.TotalProfit) / storeSummaries.Sum(x => x.TotalSales)) * 100 : 0;
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = (double)overallMargin;
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).NumberFormat = "0.00%";
                summaryRow++;
                ((Excel.Range)summarySheet.Cells[summaryRow, 1]).Value2 = "Best Performing Store:";
                var bestStore = storeSummaries.OrderByDescending(x => x.TotalProfit).First();
                ((Excel.Range)summarySheet.Cells[summaryRow, 2]).Value2 = $"{bestStore.Store} (${bestStore.TotalProfit:N2})";
                // Auto-fit columns
                summarySheet.Columns.AutoFit();
                // Sheet 3: Detailed Monthly Data
                Excel.Worksheet detailSheet = (Excel.Worksheet)workbook.Worksheets.Add();
                detailSheet.Name = "Detailed Monthly Data";
                // Headers
                ((Excel.Range)detailSheet.Cells[1, 1]).Value2 = "Month";
                ((Excel.Range)detailSheet.Cells[1, 2]).Value2 = "Store";
                ((Excel.Range)detailSheet.Cells[1, 3]).Value2 = "Profit";
                ((Excel.Range)detailSheet.Cells[1, 4]).Value2 = "Sales";
                ((Excel.Range)detailSheet.Cells[1, 5]).Value2 = "Transaction Count";
                // Style headers
                Excel.Range headerRange6 = detailSheet.Range["A1", "E1"];
                headerRange6.Font.Bold = true;
                headerRange6.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                // Populate detailed data
                row = 2;
                foreach (var item in monthlyProfit.OrderBy(x => x.Month).ThenBy(x => x.Store))
                {
                    ((Excel.Range)detailSheet.Cells[row, 1]).Value2 = item.Month.ToString("MMM yyyy");
                    ((Excel.Range)detailSheet.Cells[row, 2]).Value2 = item.Store;
                    ((Excel.Range)detailSheet.Cells[row, 3]).Value2 = (double)item.Profit;
                    ((Excel.Range)detailSheet.Cells[row, 3]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)detailSheet.Cells[row, 4]).Value2 = (double)item.Sales;
                    ((Excel.Range)detailSheet.Cells[row, 4]).NumberFormat = "$#,##0.00";
                    ((Excel.Range)detailSheet.Cells[row, 5]).Value2 = item.TransactionCount;
                    row++;
                }
                // Auto-fit columns
                detailSheet.Columns.AutoFit();
                // Save file
                workbook.SaveAs(filePath);
            }
            finally
            {
                // Clean up COM objects
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }
    }
}

