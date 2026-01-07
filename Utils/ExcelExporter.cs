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
        /// Attempts to ensure Office assemblies are available
        /// office.dll is typically in the GAC and may be version 15.0.0.0 or 16.0.0.0
        /// Try version 15.0.0.0 first since that's what's typically installed
        /// </summary>
        private static void EnsureOfficeDllLoaded()
        {
            // Try loading directly from GAC path (version 15.0.0.0) - most reliable method
            string gacPath15 = @"C:\WINDOWS\assembly\GAC_MSIL\office\15.0.0.0__71e9bce111e9429c\office.dll";
            if (System.IO.File.Exists(gacPath15))
            {
                try
                {
                    System.Reflection.Assembly.LoadFrom(gacPath15);
                    return; // Successfully loaded
                }
                catch { /* Continue to other methods */ }
            }

            // Try version 15.0.0.0 by name (Office 2013/2016)
            try
            {
                System.Reflection.Assembly.Load("office, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");
                return; // Successfully loaded
            }
            catch { /* Try version 16.0.0.0 */ }

            // Try loading from GAC path (version 16.0.0.0)
            string gacPath16 = @"C:\WINDOWS\assembly\GAC_MSIL\office\16.0.0.0__71e9bce111e9429c\office.dll";
            if (System.IO.File.Exists(gacPath16))
            {
                try
                {
                    System.Reflection.Assembly.LoadFrom(gacPath16);
                    return;
                }
                catch { /* Continue */ }
            }

            // Try version 16.0.0.0 by name (Office 2016+)
            try
            {
                System.Reflection.Assembly.Load("office, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");
                return; // Successfully loaded
            }
            catch { /* Continue */ }

            // If all attempts fail, continue anyway - dynamic typing should handle it
            // The error will be caught by try-catch blocks in the export methods
        }

        /// <summary>
        /// Converts string trendline type to Excel XlTrendlineType enum
        /// </summary>
        private static Excel.XlTrendlineType GetTrendlineType(string trendlineType)
        {
            return trendlineType?.ToLower() switch
            {
                "exponential" => Excel.XlTrendlineType.xlExponential,
                "polynomial" => Excel.XlTrendlineType.xlPolynomial,
                "power" => Excel.XlTrendlineType.xlPower,
                "moving average" => Excel.XlTrendlineType.xlMovingAvg,
                _ => Excel.XlTrendlineType.xlLinear // Default to Linear
            };
        }

        /// <summary>
        /// Applies icon set conditional formatting to a column in an Excel worksheet
        /// </summary>
        /// <param name="worksheet">The Excel worksheet to apply formatting to</param>
        /// <param name="columnLetter">The column letter (e.g., "B", "C") to apply formatting to</param>
        /// <param name="lastDataRow">The last row number containing data (1-based)</param>
        /// <param name="startConfigRow">The starting row number for configuration cells (1-based)</param>
        /// <param name="lowThresholdPercent">Low threshold percentage (default: 33)</param>
        /// <param name="highThresholdPercent">High threshold percentage (default: 67)</param>
        private static void ApplyIconSetConditionalFormatting(Excel.Worksheet worksheet, string columnLetter, int lastDataRow, int startConfigRow, int lowThresholdPercent = 33, int highThresholdPercent = 67)
        {
            if (lastDataRow < 2)
                return;

            int configRow = startConfigRow;
            
            // Header for threshold configuration
            try
            {
                dynamic thresholdHeader = worksheet.Cells[configRow, 1];
                thresholdHeader.Value2 = "Icon Set Thresholds (Percentiles)";
                try
                {
                    thresholdHeader.Font.Bold = true;
                    thresholdHeader.Font.Size = 11;
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip font formatting
                }
                catch { /* Continue if font formatting fails */ }
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                       ex is System.Runtime.InteropServices.COMException ||
                                       (ex.Message?.Contains("office") == true) ||
                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
            {
                // office.dll not available - cannot proceed
                return;
            }
            catch { /* Cannot proceed without cells access */ return; }

            // Low threshold cell
            string lowThresholdRef = "";
            try
            {
                dynamic lowThresholdLabel = worksheet.Cells[++configRow, 1];
                dynamic lowThresholdCell = worksheet.Cells[configRow, 2];
                lowThresholdLabel.Value2 = "Low Threshold (%):";
                lowThresholdCell.Value2 = lowThresholdPercent;
                lowThresholdCell.NumberFormat = "0";
                try
                {
                    lowThresholdCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip color formatting
                }
                catch { /* Continue if color formatting fails */ }
                Excel.Range lowThresholdCellTyped = (Excel.Range)lowThresholdCell;
                lowThresholdRef = lowThresholdCellTyped.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                       ex is System.Runtime.InteropServices.COMException ||
                                       (ex.Message?.Contains("office") == true) ||
                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
            {
                // office.dll not available - cannot proceed
                return;
            }
            catch { /* Cannot proceed without cells access */ return; }

            // High threshold cell
            string highThresholdRef = "";
            try
            {
                dynamic highThresholdLabel = worksheet.Cells[++configRow, 1];
                dynamic highThresholdCell = worksheet.Cells[configRow, 2];
                highThresholdLabel.Value2 = "High Threshold (%):";
                highThresholdCell.Value2 = highThresholdPercent;
                highThresholdCell.NumberFormat = "0";
                try
                {
                    highThresholdCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip color formatting
                }
                catch { /* Continue if color formatting fails */ }
                Excel.Range highThresholdCellTyped = (Excel.Range)highThresholdCell;
                highThresholdRef = highThresholdCellTyped.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                       ex is System.Runtime.InteropServices.COMException ||
                                       (ex.Message?.Contains("office") == true) ||
                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
            {
                // office.dll not available - cannot proceed
                return;
            }
            catch { /* Cannot proceed without cells access */ return; }

            // Helper formula cells
            string lowValueRef = "";
            string highValueRef = "";
            try
            {
                configRow++;
                dynamic lowValueLabel = worksheet.Cells[configRow, 1];
                dynamic lowValueCell = worksheet.Cells[configRow, 2];
                lowValueLabel.Value2 = "Low Threshold Value:";
                lowValueCell.Formula = $"=PERCENTILE(${columnLetter}$2:${columnLetter}${lastDataRow},{lowThresholdRef}/100)";
                lowValueCell.NumberFormat = "$#,##0.00";
                Excel.Range lowValueCellTyped = (Excel.Range)lowValueCell;
                lowValueRef = lowValueCellTyped.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);

                configRow++;
                dynamic highValueLabel = worksheet.Cells[configRow, 1];
                dynamic highValueCell = worksheet.Cells[configRow, 2];
                highValueLabel.Value2 = "High Threshold Value:";
                highValueCell.Formula = $"=PERCENTILE(${columnLetter}$2:${columnLetter}${lastDataRow},{highThresholdRef}/100)";
                highValueCell.NumberFormat = "$#,##0.00";
                Excel.Range highValueCellTyped = (Excel.Range)highValueCell;
                highValueRef = highValueCellTyped.get_Address(true, false, Excel.XlReferenceStyle.xlA1, false, null);
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                       ex is System.Runtime.InteropServices.COMException ||
                                       (ex.Message?.Contains("office") == true) ||
                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
            {
                // office.dll not available - cannot proceed
                return;
            }
            catch { /* Cannot proceed without cells access */ return; }

            // Apply icon set conditional formatting to the specified column
            dynamic dataRange = worksheet.Range[$"{columnLetter}2:{columnLetter}{lastDataRow}"];

            dynamic iconSetCondition = null;
            try
            {
                iconSetCondition = dataRange.FormatConditions.AddIconSetCondition();
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                       ex is System.Runtime.InteropServices.COMException ||
                                       (ex.Message?.Contains("office") == true) ||
                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
            {
                // office.dll not available - cannot apply conditional formatting
                // Return early since we can't proceed without FormatConditions
                return;
            }
            catch
            {
                // If FormatConditions fails, return early
                return;
            }

            if (iconSetCondition == null)
            {
                return;
            }

            // Set the icon set to 3 Traffic Lights
            try
            {
                dynamic worksheetDynamic = worksheet;
                dynamic app = worksheetDynamic.Application;
                dynamic iconSets = app.IconSets;
                iconSetCondition.IconSet = iconSets[Excel.XlIconSet.xl3TrafficLights1];
            }
            catch
            {
                // If setting IconSet fails, Excel will use default icon set
            }

            // Set icon criteria thresholds using user-provided percent values
            // For 3-icon sets: IconCriteria(2) is the lower threshold, IconCriteria(3) is the upper threshold
            // Icon distribution:
            //   - Red (icon 1): values < lowThresholdPercent percentile
            //   - Yellow (icon 2): values >= lowThresholdPercent and < highThresholdPercent percentile
            //   - Green (icon 3): values >= highThresholdPercent percentile
            try
            {
                dynamic iconCriteria = iconSetCondition.IconCriteria;
                
                // Set the lower threshold (IconCriteria(2)) - values below this get red icon
                dynamic criterion2 = iconCriteria[2];
                criterion2.Type = Excel.XlConditionValueTypes.xlConditionValuePercent;
                criterion2.Value = lowThresholdPercent;
                criterion2.Operator = 5; // xlGreaterEqual = 5
                
                // Set the upper threshold (IconCriteria(3)) - values at or above this get green icon
                dynamic criterion3 = iconCriteria[3];
                criterion3.Type = Excel.XlConditionValueTypes.xlConditionValuePercent;
                criterion3.Value = highThresholdPercent;
                criterion3.Operator = 5; // xlGreaterEqual = 5
            }
            catch
            {
                // Fallback: If setting percent thresholds fails, try using calculated absolute values
                try
                {
                    // Read the calculated threshold values from the helper cells
                    dynamic lowValueCellRef = worksheet.Cells[configRow - 1, 2];
                    dynamic highValueCellRef = worksheet.Cells[configRow, 2];
                    double lowThresholdValue = (double)lowValueCellRef.Value2;
                    double highThresholdValue = (double)highValueCellRef.Value2;

                    dynamic iconCriteria = iconSetCondition.IconCriteria;
                    
                    // Set the lower threshold using absolute number
                    dynamic criterion2 = iconCriteria[2];
                    criterion2.Type = Excel.XlConditionValueTypes.xlConditionValueNumber;
                    criterion2.Value = lowThresholdValue;
                    criterion2.Operator = 5; // xlGreaterEqual = 5
                    
                    // Set the upper threshold using absolute number
                    dynamic criterion3 = iconCriteria[3];
                    criterion3.Type = Excel.XlConditionValueTypes.xlConditionValueNumber;
                    criterion3.Value = highThresholdValue;
                    criterion3.Operator = 5; // xlGreaterEqual = 5
                }
                catch
                {
                    // If all attempts fail, Excel will use default criteria (33%, 67%)
                }
            }

            // Add helpful note for users
            configRow++;
            try
            {
                dynamic noteCell = worksheet.Cells[configRow, 1];
                noteCell.Value2 = $"To enable auto-update: Edit CF rule and reference cells {lowValueRef} and {highValueRef}";
                try
                {
                    noteCell.Font.Italic = true;
                    noteCell.Font.Size = 9;
                    noteCell.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Gray);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip font formatting
                }
                catch { /* Continue if font formatting fails */ }
                noteCell.WrapText = true;
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                       ex is System.Runtime.InteropServices.COMException ||
                                       (ex.Message?.Contains("office") == true) ||
                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
            {
                // office.dll not available - skip note
            }
            catch { /* Continue if note fails */ }
        }
        /// <summary>
        /// Exports future sales estimation data to Excel with descriptive and predictive analytics
        /// </summary>
        /// <param name="filePath">Path where the Excel file will be saved</param>
        /// <param name="filteredData">Filtered sales data to export</param>
        /// <param name="forecastMonths">Number of months to forecast</param>
        /// <param name="trendlineType">Type of trendline to use (Linear, Exponential, Polynomial, Power, Moving Average)</param>
        /// <param name="lowThresholdPercent">Low threshold percentage for icon set formatting (default: 33)</param>
        /// <param name="highThresholdPercent">High threshold percentage for icon set formatting (default: 67)</param>
        /// <param name="polynomialOrder">Order for polynomial trendline (default: 2, range 2-6)</param>
        /// <param name="movingAveragePeriod">Period for moving average trendline (default: 3, minimum 2)</param>
        public static void ExportFutureSalesToExcel(string filePath, List<SalesRecord> filteredData, int forecastMonths, string trendlineType = "Linear", int lowThresholdPercent = 33, int highThresholdPercent = 67, int polynomialOrder = 2, int movingAveragePeriod = 3)
        {
            if (filteredData == null || !filteredData.Any())
            {
                MessageBox.Show("No data available to export.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Attempt to load office.dll before creating Excel Application
            EnsureOfficeDllLoaded();

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            try
            {
                excelApp = new Excel.Application();
                try
                {
                    dynamic app = excelApp;
                    app.Visible = false;
                    app.DisplayAlerts = false;
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - try to continue with defaults
                }
                catch { /* Continue if property access fails */ }
                
                dynamic workbooks = excelApp.Workbooks;
                workbook = (Excel.Workbook)workbooks.Add();

                // Sheet 1: Monthly Sales Data (Descriptive Analytics)
                dynamic worksheets = workbook.Worksheets;
                dynamic monthlySheet = worksheets[1];
                monthlySheet.Name = "Monthly Sales Data";

                // Headers
                try
                {
                    dynamic cell1 = monthlySheet.Cells[1, 1];
                    cell1.Value2 = "Month";
                    dynamic cell2 = monthlySheet.Cells[1, 2];
                    cell2.Value2 = "Total Sales";
                    dynamic cell3 = monthlySheet.Cells[1, 3];
                    cell3.Value2 = "Transaction Count";
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip header data
                }
                catch { /* Continue if header data fails */ }

                // Style headers
                try
                {
                    dynamic headerRange1 = monthlySheet.Range["A1", "C1"];
                    headerRange1.Font.Bold = true;
                    headerRange1.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip header formatting
                }
                catch { /* Continue if header formatting fails */ }

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
                    try
                    {
                        dynamic cell1 = monthlySheet.Cells[row, 1];
                        cell1.Value2 = item.Month.ToString("MMM yyyy");
                        dynamic cell2 = monthlySheet.Cells[row, 2];
                        cell2.Value2 = (double)item.TotalSales;
                        cell2.NumberFormat = "$#,##0.00";
                        dynamic cell3 = monthlySheet.Cells[row, 3];
                        cell3.Value2 = item.TransactionCount;
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip this row
                    }
                    catch { /* Continue if row data fails */ }
                    row++;
                }

                // Add summary statistics (Descriptive Analytics)
                int summaryRow = row + 2;
                try
                {
                    dynamic summaryCell = monthlySheet.Cells[summaryRow, 1];
                    summaryCell.Value2 = "SUMMARY STATISTICS";
                    try
                    {
                        summaryCell.Font.Bold = true;
                        summaryCell.Font.Size = 12;
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip font formatting
                    }
                    catch { /* Continue if font formatting fails */ }
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip summary header
                }
                catch { /* Continue if summary header fails */ }
                summaryRow++;
                try
                {
                    dynamic cell1 = monthlySheet.Cells[summaryRow, 1];
                    cell1.Value2 = "Total Sales:";
                    dynamic cell2 = monthlySheet.Cells[summaryRow, 2];
                    cell2.Value2 = (double)monthlySales.Sum(x => x.TotalSales);
                    cell2.NumberFormat = "$#,##0.00";
                    try
                    {
                        cell2.Font.Bold = true;
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip font formatting
                    }
                    catch { /* Continue if font formatting fails */ }
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip this row
                }
                catch { /* Continue if row fails */ }
                summaryRow++;
                try
                {
                    dynamic cell1 = monthlySheet.Cells[summaryRow, 1];
                    cell1.Value2 = "Average Monthly Sales:";
                    dynamic cell2 = monthlySheet.Cells[summaryRow, 2];
                    cell2.Value2 = (double)monthlySales.Average(x => x.TotalSales);
                    cell2.NumberFormat = "$#,##0.00";
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip this row
                }
                catch { /* Continue if row fails */ }
                summaryRow++;
                try
                {
                    dynamic cell1 = monthlySheet.Cells[summaryRow, 1];
                    cell1.Value2 = "Maximum Monthly Sales:";
                    dynamic cell2 = monthlySheet.Cells[summaryRow, 2];
                    cell2.Value2 = (double)monthlySales.Max(x => x.TotalSales);
                    cell2.NumberFormat = "$#,##0.00";
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip this row
                }
                catch { /* Continue if row fails */ }
                summaryRow++;
                try
                {
                    dynamic cell1 = monthlySheet.Cells[summaryRow, 1];
                    cell1.Value2 = "Minimum Monthly Sales:";
                    dynamic cell2 = monthlySheet.Cells[summaryRow, 2];
                    cell2.Value2 = (double)monthlySales.Min(x => x.TotalSales);
                    cell2.NumberFormat = "$#,##0.00";
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip this row
                }
                catch { /* Continue if row fails */ }
                summaryRow++;
                try
                {
                    dynamic cell1 = monthlySheet.Cells[summaryRow, 1];
                    cell1.Value2 = "Total Transactions:";
                    dynamic cell2 = monthlySheet.Cells[summaryRow, 2];
                    cell2.Value2 = monthlySales.Sum(x => x.TransactionCount);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip this row
                }
                catch { /* Continue if row fails */ }

                // Apply icon set conditional formatting to Total Sales column (Column B)
                ApplyIconSetConditionalFormatting((Excel.Worksheet)monthlySheet, "B", row - 1, summaryRow + 3, lowThresholdPercent, highThresholdPercent);

                // Auto-fit columns
                try
                {
                    dynamic columns = monthlySheet.Columns;
                    columns.AutoFit();
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip auto-fit
                }
                catch { /* Continue if auto-fit fails */ }

                // Sheet 2: Forecast Data (Predictive Analytics)
                dynamic worksheets2 = workbook.Worksheets;
                dynamic forecastSheet = worksheets2.Add();
                forecastSheet.Name = "Forecast Data";

                // Headers
                try
                {
                    dynamic cell1 = forecastSheet.Cells[1, 1];
                    cell1.Value2 = "Date";
                    dynamic cell2 = forecastSheet.Cells[1, 2];
                    cell2.Value2 = "Type";
                    dynamic cell3 = forecastSheet.Cells[1, 3];
                    cell3.Value2 = "Sales Forecast";
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip header data
                }
                catch { /* Continue if header data fails */ }

                // Style headers
                try
                {
                    dynamic headerRange2 = forecastSheet.Range["A1", "C1"];
                    headerRange2.Font.Bold = true;
                    headerRange2.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGreen);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip header formatting
                }
                catch { /* Continue if header formatting fails */ }

                // Get forecast data
                // NOTE: Uses filteredData which respects all global filters (Date Range, Store, Product)
                var trendAndForecast = SalesUtility.CalculateTrendAndForecast(filteredData, forecastMonths);

                // Populate forecast data
                row = 2;
                foreach (var point in trendAndForecast)
                {
                    try
                    {
                        dynamic cell1 = forecastSheet.Cells[row, 1];
                        cell1.Value2 = point.Date;
                        cell1.NumberFormat = "MMM yyyy";
                        dynamic cell2 = forecastSheet.Cells[row, 2];
                        cell2.Value2 = point.IsForecast ? "Forecast" : "Historical Trend";
                        dynamic cell3 = forecastSheet.Cells[row, 3];
                        cell3.Value2 = (double)point.Value;
                        cell3.NumberFormat = "$#,##0.00";

                        // Highlight forecast rows
                        if (point.IsForecast)
                        {
                            try
                            {
                                dynamic forecastRowRange = forecastSheet.Range[forecastSheet.Cells[row, 1], forecastSheet.Cells[row, 3]];
                                forecastRowRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightYellow);
                                cell2.Font.Italic = true;
                            }
                            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                       ex is System.Runtime.InteropServices.COMException ||
                                                       (ex.Message?.Contains("office") == true) ||
                                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
                            {
                                // office.dll not available - skip row formatting
                            }
                            catch { /* Continue if row formatting fails */ }
                        }
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip this row
                    }
                    catch { /* Continue if row fails */ }
                    row++;
                }

                // Add forecast summary
                var forecastPoints = trendAndForecast.Where(p => p.IsForecast).ToList();
                if (forecastPoints.Any())
                {
                    summaryRow = row + 2;
                    try
                    {
                        dynamic summaryCell = forecastSheet.Cells[summaryRow, 1];
                        summaryCell.Value2 = "FORECAST SUMMARY";
                        try
                        {
                            summaryCell.Font.Bold = true;
                            summaryCell.Font.Size = 12;
                        }
                        catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                   ex is System.Runtime.InteropServices.COMException ||
                                                   (ex.Message?.Contains("office") == true) ||
                                                   (ex.Message?.Contains("71e9bce111e9429c") == true))
                        {
                            // office.dll not available - skip font formatting
                        }
                        catch { /* Continue if font formatting fails */ }
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip summary header
                    }
                    catch { /* Continue if summary header fails */ }
                    summaryRow++;
                    try
                    {
                        dynamic cell1 = forecastSheet.Cells[summaryRow, 1];
                        cell1.Value2 = "Forecast Period:";
                        dynamic cell2 = forecastSheet.Cells[summaryRow, 2];
                        cell2.Value2 = $"{forecastMonths} months";
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip this row
                    }
                    catch { /* Continue if row fails */ }
                    summaryRow++;
                    try
                    {
                        dynamic cell1 = forecastSheet.Cells[summaryRow, 1];
                        cell1.Value2 = "Total Forecasted Sales:";
                        dynamic cell2 = forecastSheet.Cells[summaryRow, 2];
                        cell2.Value2 = (double)forecastPoints.Sum(p => p.Value);
                        cell2.NumberFormat = "$#,##0.00";
                        try
                        {
                            cell2.Font.Bold = true;
                        }
                        catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                   ex is System.Runtime.InteropServices.COMException ||
                                                   (ex.Message?.Contains("office") == true) ||
                                                   (ex.Message?.Contains("71e9bce111e9429c") == true))
                        {
                            // office.dll not available - skip font formatting
                        }
                        catch { /* Continue if font formatting fails */ }
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip this row
                    }
                    catch { /* Continue if row fails */ }
                    summaryRow++;
                    try
                    {
                        dynamic cell1 = forecastSheet.Cells[summaryRow, 1];
                        cell1.Value2 = "Average Monthly Forecast:";
                        dynamic cell2 = forecastSheet.Cells[summaryRow, 2];
                        cell2.Value2 = (double)forecastPoints.Average(p => p.Value);
                        cell2.NumberFormat = "$#,##0.00";
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip this row
                    }
                    catch { /* Continue if row fails */ }
                }
                else
                {
                    // If no forecast points, set summaryRow to after the data
                    summaryRow = row + 2;
                }

                // Apply icon set conditional formatting to Sales Forecast column (Column C)
                ApplyIconSetConditionalFormatting((Excel.Worksheet)forecastSheet, "C", row - 1, summaryRow + 3, lowThresholdPercent, highThresholdPercent);

                // Auto-fit columns
                try
                {
                    dynamic columns = forecastSheet.Columns;
                    columns.AutoFit();
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip auto-fit
                }
                catch { /* Continue if auto-fit fails */ }

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
                    try
                    {
                        dynamic cell1 = forecastSheet.Cells[forecastDataRow, 1];
                        dynamic cell2 = forecastSheet.Cells[forecastDataRow, 2];
                        dynamic cell3 = forecastSheet.Cells[forecastDataRow, 3];
                        object dateObj = cell1.Value2;
                        object typeObj = cell2.Value2;
                        object salesObj = cell3.Value2;

                        // Stop if we hit an empty row
                        if (dateObj == null || typeObj == null || salesObj == null)
                            break;

                        DateTime date = DateTime.FromOADate((double)dateObj);
                        string type = typeObj.ToString();
                        double salesForecast = (double)salesObj;

                        forecastDataList.Add((date, type, salesForecast));
                        forecastDataRow++;
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - stop reading
                        break;
                    }
                    catch { /* Stop reading if access fails */ break; }
                }

                // Populate chart data: Month, Actual Sales, Actual Sales Line (for trendline), Forecast (Dots)
                int chartDataRow = 1;

                try
                {
                    dynamic cell1 = chartDataSheet.Cells[chartDataRow, 1];
                    cell1.Value2 = "Month";
                    dynamic cell2 = chartDataSheet.Cells[chartDataRow, 2];
                    cell2.Value2 = "Actual Sales";
                    dynamic cell3 = chartDataSheet.Cells[chartDataRow, 3];
                    cell3.Value2 = "Actual Sales Line";
                    dynamic cell4 = chartDataSheet.Cells[chartDataRow, 4];
                    cell4.Value2 = "Forecast";
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - skip chart data headers
                }
                catch { /* Continue if chart data headers fail */ }

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

                    try
                    {
                        dynamic cell1 = chartDataSheet.Cells[chartDataRow, 1];
                        cell1.Value2 = normalizedMonth;
                        cell1.NumberFormat = "MMM yyyy";

                        // Actual Sales (only for months with actual sales data)
                        var actualSales = monthlySalesSummary.FirstOrDefault(m =>
                            new DateTime(m.Month.Year, m.Month.Month, 1) == normalizedMonth);
                        dynamic cell2 = chartDataSheet.Cells[chartDataRow, 2];
                        if (actualSales != null)
                        {
                            cell2.Value2 = (double)actualSales.TotalSales;

                            // Track historical data rows for trendline calculation
                            if (firstHistoricalRow == 0) firstHistoricalRow = chartDataRow;
                            lastHistoricalRow = chartDataRow;
                        }
                        else
                        {
                            cell2.Value2 = "";
                        }

                        // Column 3: Actual Sales Line (same as Column 2, but for line series - used for trendline calculation)
                        // This will be invisible but needed for Excel to calculate the trendline
                        // IMPORTANT: Only populate for historical months (not forecast months)
                        dynamic cell3 = chartDataSheet.Cells[chartDataRow, 3];
                        if (actualSales != null)
                        {
                            cell3.Value2 = (double)actualSales.TotalSales;
                        }
                        else
                        {
                            // Leave empty for forecast months - this ensures trendline only uses historical data
                            cell3.Value2 = "";
                        }

                        // Column 4: Forecast - use Excel's FORECAST.LINEAR function for forecast months
                        // Only calculate forecast for months after the last historical month
                        bool isForecastMonth = lastHistoricalMonth.HasValue && normalizedMonth > lastHistoricalMonth.Value;

                        dynamic cell4 = chartDataSheet.Cells[chartDataRow, 4];
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

                            cell4.Formula = forecastFormula;
                        }
                        else
                        {
                            cell4.Value2 = "";
                        }
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip this row
                    }
                    catch { /* Continue if row fails */ }

                    chartDataRow++;
                }

                // Create chart from the data sheet - check if office.dll is available first
                int lastRow = chartDataRow - 1;
                
                // Check if office.dll is available before attempting chart creation
                bool canCreateCharts = false;
                try
                {
                    // Try to resolve the office assembly - try version 15.0.0.0 first (installed version)
                    string gacPath15 = @"C:\WINDOWS\assembly\GAC_MSIL\office\15.0.0.0__71e9bce111e9429c\office.dll";
                    if (System.IO.File.Exists(gacPath15))
                    {
                        var officeAssembly = System.Reflection.Assembly.LoadFrom(gacPath15);
                        canCreateCharts = officeAssembly != null;
                    }
                    else
                    {
                        // Try loading by name
                        var officeAssembly = System.Reflection.Assembly.Load("office, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");
                        canCreateCharts = officeAssembly != null;
                    }
                }
                catch
                {
                    // Try version 16.0.0.0 as fallback
                    try
                    {
                        var officeAssembly = System.Reflection.Assembly.Load("office, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");
                        canCreateCharts = officeAssembly != null;
                    }
                    catch
                    {
                        // office.dll not available - skip chart creation
                        canCreateCharts = false;
                    }
                }
                
                if (canCreateCharts)
                {
                    try
                    {
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
                            try
                            {
                                dynamic actualSeriesFormat = actualSeries.Format;
                                dynamic actualSeriesFill = actualSeriesFormat.Fill;
                                actualSeriesFill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);
                            }
                            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                       ex is System.Runtime.InteropServices.COMException ||
                                                       (ex.Message?.Contains("office") == true) ||
                                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
                            {
                                // office.dll not available - skip color formatting
                            }
                            catch { /* Continue if color formatting fails */ }
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
                            try
                            {
                                dynamic actualSalesLineFormat = actualSalesLineSeries.Format;
                                dynamic actualSalesLineLine = actualSalesLineFormat.Line;
                                actualSalesLineLine.Visible = 0; // Hide the line
                            }
                            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                       ex is System.Runtime.InteropServices.COMException ||
                                                       (ex.Message?.Contains("office") == true) ||
                                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
                            {
                                // office.dll not available - skip line visibility
                            }
                            catch { /* Continue if line visibility fails */ }
                            actualSalesLineSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone; // No markers

                            // Add trendline to this series - only for historical data (no forward extension)
                            Excel.Trendlines trendlines = (Excel.Trendlines)actualSalesLineSeries.Trendlines();
                            Excel.XlTrendlineType trendlineTypeEnum = GetTrendlineType(trendlineType);
                            Excel.Trendline trendline = (Excel.Trendline)trendlines.Add(trendlineTypeEnum);

                            // Apply extra options based on trendline type
                            if (trendlineTypeEnum == Excel.XlTrendlineType.xlPolynomial)
                            {
                                int order = Math.Max(2, Math.Min(6, polynomialOrder));
                                trendline.Order = order;
                            }
                            else if (trendlineTypeEnum == Excel.XlTrendlineType.xlMovingAvg)
                            {
                                int period = Math.Max(2, movingAveragePeriod);
                                trendline.Period = period;
                            }

                            // Configure trendline - only shows historical trend, not forecast
                            trendline.Name = "Trend";
                            try
                            {
                                dynamic trendlineFormat = trendline.Format;
                                dynamic trendlineLine = trendlineFormat.Line;
                                trendlineLine.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(Color.Red);
                                trendlineLine.Weight = 3;
                            }
                            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                       ex is System.Runtime.InteropServices.COMException ||
                                                       (ex.Message?.Contains("office") == true) ||
                                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
                            {
                                // office.dll not available - skip trendline formatting
                            }
                            catch { /* Continue if trendline formatting fails */ }

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
                            try
                            {
                                dynamic forecastSeriesFormat = forecastSeries.Format;
                                dynamic forecastSeriesLine = forecastSeriesFormat.Line;
                                forecastSeriesLine.Visible = 0; // 0 = msoFalse (invisible line)
                            }
                            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                       ex is System.Runtime.InteropServices.COMException ||
                                                       (ex.Message?.Contains("office") == true) ||
                                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
                            {
                                // office.dll not available - skip line visibility
                            }
                            catch { /* Continue if line visibility fails */ }

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
                            try
                            {
                                dynamic forecastSeriesFormat = forecastSeries.Format;
                                dynamic forecastSeriesLine = forecastSeriesFormat.Line;
                                forecastSeriesLine.Visible = 0; // 0 = msoFalse (invisible line)
                            }
                            catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                                       ex is System.Runtime.InteropServices.COMException ||
                                                       (ex.Message?.Contains("office") == true) ||
                                                       (ex.Message?.Contains("71e9bce111e9429c") == true))
                            {
                                // office.dll not available - skip line visibility
                            }
                            catch { /* Continue if line visibility fails */ }

                            // Configure markers (dots) only
                            forecastSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleCircle;
                            forecastSeries.MarkerSize = 8; // Larger dots for better visibility

                            // Set marker color to red
                            forecastSeries.MarkerForegroundColor = System.Drawing.ColorTranslator.ToOle(Color.Red);
                            forecastSeries.MarkerBackgroundColor = System.Drawing.ColorTranslator.ToOle(Color.Red);
                        }

                    // Chart title and formatting - match excelPredictive branch design
                    try
                    {
                        chartSheet.HasTitle = true;
                        dynamic chartTitle = chartSheet.ChartTitle;
                        chartTitle.Text = "Future Sales Estimation (Monthly Revenue Trend)";
                        dynamic chartTitleFont = chartTitle.Font;
                        chartTitleFont.Size = 14;
                        chartTitleFont.Bold = true;
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip chart title formatting
                    }
                    catch { /* Continue if chart title fails */ }

                    // Axis titles - match excelPredictive branch design
                    try
                    {
                        Excel.Axis categoryAxis = (Excel.Axis)chartSheet.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                        categoryAxis.HasTitle = true;
                        dynamic categoryAxisTitle = categoryAxis.AxisTitle;
                        categoryAxisTitle.Text = "Month";
                        dynamic categoryAxisTitleFont = categoryAxisTitle.Font;
                        categoryAxisTitleFont.Size = 11;
                        categoryAxisTitleFont.Bold = true;
                        
                        // Format category axis dates
                        categoryAxis.CategoryType = Excel.XlCategoryType.xlCategoryScale;
                        categoryAxis.TickLabels.NumberFormat = "MMM yy";
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip category axis formatting
                    }
                    catch { /* Continue if category axis fails */ }
                    
                    try
                    {
                        Excel.Axis valueAxis = (Excel.Axis)chartSheet.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                        valueAxis.HasTitle = true;
                        dynamic valueAxisTitle = valueAxis.AxisTitle;
                        valueAxisTitle.Text = "Total Sales (Revenue)";
                        dynamic valueAxisTitleFont = valueAxisTitle.Font;
                        valueAxisTitleFont.Size = 11;
                        valueAxisTitleFont.Bold = true;
                        
                        // Format value axis as currency
                        valueAxis.TickLabels.NumberFormat = "$#,##0";
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip value axis formatting
                    }
                    catch { /* Continue if value axis fails */ }
                    
                    // Legend - match excelPredictive branch design
                    try
                    {
                        chartSheet.HasLegend = true;
                        dynamic legend = chartSheet.Legend;
                        legend.Position = Excel.XlLegendPosition.xlLegendPositionTop;
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - use default legend position
                    }
                    catch { /* Legend access failed - continue with default */ }

                    // Chart area formatting - match excelPredictive branch design
                    try
                    {
                        dynamic plotArea = chartSheet.PlotArea;
                        dynamic plotAreaFormat = plotArea.Format;
                        dynamic plotAreaFill = plotAreaFormat.Fill;
                        plotAreaFill.Visible = 0; // 0 = msoFalse (transparent)
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip plot area formatting
                    }
                    catch { /* Continue if plot area formatting fails */ }

                    try
                    {
                        dynamic chartArea = chartSheet.ChartArea;
                        dynamic chartAreaFormat = chartArea.Format;
                        dynamic chartAreaFill = chartAreaFormat.Fill;
                        chartAreaFill.Visible = 0; // 0 = msoFalse (transparent)
                    }
                    catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                               ex is System.Runtime.InteropServices.COMException ||
                                               (ex.Message?.Contains("office") == true) ||
                                               (ex.Message?.Contains("71e9bce111e9429c") == true))
                    {
                        // office.dll not available - skip chart area formatting
                    }
                    catch { /* Continue if chart area formatting fails */ }

                    // Move chart to its own sheet
                    try
                    {
                        chartSheet.Location(Excel.XlChartLocation.xlLocationAsNewSheet, "Sales Trend Chart");
                    }
                    catch
                    {
                        // If moving fails, chart stays embedded - still functional
                    }
                    
                    // Hide the temporary data sheet
                    try
                    {
                        chartDataSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    }
                    catch { /* Continue if hiding fails */ }
                }
                catch (System.IO.FileNotFoundException ex) when (ex.Message?.Contains("office") == true || ex.Message?.Contains("71e9bce111e9429c") == true)
                {
                    // office.dll not available - skip chart creation, but continue with data export
                    // Data is already exported to sheets, so the export is still useful
                    try
                    {
                        chartDataSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    }
                    catch { /* Continue if hiding fails */ }
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // COM error - skip chart creation
                    try
                    {
                        chartDataSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    }
                    catch { /* Continue if hiding fails */ }
                }
                catch (Exception ex) when (ex.Message?.Contains("office") == true || ex.Message?.Contains("71e9bce111e9429c") == true)
                {
                    // office.dll related error - skip chart creation
                    try
                    {
                        chartDataSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    }
                    catch { /* Continue if hiding fails */ }
                    }
                }
                else
                {
                    // office.dll not available - skip chart creation entirely
                    // Just hide the chart data sheet
                    try
                    {
                        chartDataSheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    }
                    catch { /* Continue if hiding fails */ }
                }

                // Save file
                try
                {
                    dynamic wb = workbook;
                    wb.SaveAs(filePath);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - try alternative save method
                    try
                    {
                        workbook.SaveAs(filePath);
                    }
                    catch
                    {
                        throw new Exception("Unable to save Excel file. Office may not be properly installed.");
                    }
                }
                catch { /* Try alternative save */ }
            }
            finally
            {
                // Clean up COM objects
                if (workbook != null)
                {
                    try
                    {
                        dynamic wb = workbook;
                        wb.Close(false);
                    }
                    catch { /* Continue cleanup */ }
                    try
                    {
                        workbook.Close(false);
                    }
                    catch { /* Continue cleanup */ }
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    try
                    {
                        dynamic app = excelApp;
                        app.Quit();
                    }
                    catch { /* Continue cleanup */ }
                    try
                    {
                        excelApp.Quit();
                    }
                    catch { /* Continue cleanup */ }
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

            // Attempt to load office.dll before creating Excel Application
            EnsureOfficeDllLoaded();

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
                try
                {
                    dynamic wb = workbook;
                    wb.SaveAs(filePath);
                }
                catch (Exception ex) when (ex is System.IO.FileNotFoundException || 
                                           ex is System.Runtime.InteropServices.COMException ||
                                           (ex.Message?.Contains("office") == true) ||
                                           (ex.Message?.Contains("71e9bce111e9429c") == true))
                {
                    // office.dll not available - try alternative save method
                    try
                    {
                        workbook.SaveAs(filePath);
                    }
                    catch
                    {
                        throw new Exception("Unable to save Excel file. Office may not be properly installed.");
                    }
                }
                catch { /* Try alternative save */ }
            }
            finally
            {
                // Clean up COM objects
                if (workbook != null)
                {
                    try
                    {
                        dynamic wb = workbook;
                        wb.Close(false);
                    }
                    catch { /* Continue cleanup */ }
                    try
                    {
                        workbook.Close(false);
                    }
                    catch { /* Continue cleanup */ }
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    try
                    {
                        dynamic app = excelApp;
                        app.Quit();
                    }
                    catch { /* Continue cleanup */ }
                    try
                    {
                        excelApp.Quit();
                    }
                    catch { /* Continue cleanup */ }
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }
    }
}

