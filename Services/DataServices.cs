using BakeryBI.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBI.Services
{
        public class DataService
        {
            private List<Sale> allSales;

            public DataService()
            {
                allSales = new List<Sale>();
            }

            public List<Sale> LoadSalesData(string filePath)
            {
                try
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        MissingFieldFound = null,
                        BadDataFound = null
                    };

                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        allSales = csv.GetRecords<Sale>().ToList();
                    }

                    return allSales;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error loading data: {ex.Message}");
                }
            }

            public List<Sale> GetAllSales()
            {
                return allSales;
            }

        public List<string> GetUniqueStores()
        {
            if (allSales == null || !allSales.Any())
                return new List<string>();

            return allSales.Select(s => s.store_name)
                          .Distinct()
                          .OrderBy(s => s)
                          .ToList();
        }

        public List<string> GetUniqueCategories()
        {
            if (allSales == null || !allSales.Any())
                return new List<string>();

            return allSales.Select(s => s.aisle)
                          .Distinct()
                          .OrderBy(s => s)
                          .ToList();
        }

        public List<string> GetUniqueProducts()
            {
                return allSales.Select(s => s.product_name)
                              .Distinct()
                              .OrderBy(s => s)
                              .ToList();
            }

            public List<int> GetUniqueYears()
            {
                return allSales.Select(s => s.year)
                              .Distinct()
                              .OrderBy(y => y)
                              .ToList();
            }

            public List<string> GetCustomerTypes()
            {
                return allSales.Select(s => s.customer_type)
                              .Distinct()
                              .OrderBy(c => c)
                              .ToList();
            }

            public List<Sale> FilterSales(string store = null, string category = null,
                                          string customerType = null, int? year = null,
                                          int? month = null)
            {
                var filtered = allSales.AsEnumerable();

                if (!string.IsNullOrEmpty(store) && store != "All Stores")
                    filtered = filtered.Where(s => s.store_name == store);

                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    filtered = filtered.Where(s => s.aisle == category);

                if (!string.IsNullOrEmpty(customerType) && customerType != "All Types")
                    filtered = filtered.Where(s => s.customer_type == customerType);

                if (year.HasValue && year.Value != 0)
                    filtered = filtered.Where(s => s.year == year.Value);

                if (month.HasValue && month.Value != 0)
                    filtered = filtered.Where(s => s.month == month.Value);

                return filtered.ToList();
            }
        }
    }

