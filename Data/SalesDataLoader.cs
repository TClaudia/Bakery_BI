using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BakeryBI.Data
{
    public class SalesDataLoader
    {
        public List<SalesRecord> LoadFromCsv(string filePath)
        {
            var records = new List<SalesRecord>();

            try
            {
                var lines = File.ReadAllLines(filePath);

                // Skip header (first line)
                for (int i = 1; i < lines.Length; i++)
                {
                    try
                    {
                        var values = ParseCsvLine(lines[i]);

                        if (values.Length >= 22)
                        {
                            var record = new SalesRecord
                            {
                                CustomerId = values[0],
                                StoreName = values[1],
                                TransactionDate = DateTime.Parse(values[2]),
                                Aisle = values[3],
                                ProductName = values[4],
                                Quantity = int.Parse(values[5]),
                                UnitPrice = decimal.Parse(values[6]),
                                TotalAmount = decimal.Parse(values[7]),
                                DiscountAmount = decimal.Parse(values[8]),
                                FinalAmount = decimal.Parse(values[9]),
                                LoyaltyPoints = int.Parse(values[10]),
                                UnitCost = decimal.Parse(values[11]),
                                TotalCost = decimal.Parse(values[12]),
                                Profit = decimal.Parse(values[13]),
                                ProfitMargin = decimal.Parse(values[14]),
                                Year = int.Parse(values[15]),
                                Month = int.Parse(values[16]),
                                MonthName = values[17],
                                Quarter = int.Parse(values[18]),
                                DayOfWeek = values[19],
                                WeekNumber = int.Parse(values[20]),
                                CustomerType = values[21]
                            };

                            records.Add(record);
                        }
                    }
                    catch
                    {
                        
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading CSV file: {ex.Message}", ex);
            }

            return records;
        }

        private string[] ParseCsvLine(string line)
        {
            var values = new List<string>();
            bool inQuotes = false;
            string currentValue = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue.Trim());
                    currentValue = "";
                }
                else
                {
                    currentValue += c;
                }
            }

            values.Add(currentValue.Trim());
            return values.ToArray();
        }
    }
}