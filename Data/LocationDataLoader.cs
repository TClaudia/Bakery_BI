using System;
using System.Collections.Generic;
using System.Linq;

namespace BakeryBI.Data
{
    public class LocationDataLoader
    {
        public List<StoreLocation> GetStoreLocations(List<SalesRecord> salesData)
        {
            if (salesData == null || !salesData.Any())
                return new List<StoreLocation>();

            var locations = salesData
                .GroupBy(r => new { r.StoreName })
                .Select(g =>
                {
                    var firstRecord = g.First();
                    var city = GetCityFromStoreName(firstRecord.StoreName);
                    var coords = StoreLocation.GetCityCoordinates(city);

                    return new StoreLocation
                    {
                        StoreName = g.Key.StoreName,
                        City = city,
                        Country = "Romania",
                        Latitude = coords.lat,
                        Longitude = coords.lon,
                        TotalSales = g.Sum(r => r.FinalAmount),
                        TransactionCount = g.Count()
                    };
                })
                .ToList();

            return locations;
        }

        private string GetCityFromStoreName(string storeName)
        {
            return storeName switch
            {
                "ValuePlus Market" => "Bucuresti",
                "SuperSave Central" => "Cluj-Napoca",
                "Corner Grocery" => "Iasi",
                "City Fresh Store" => "Timisoara",
                "MegaMart Westside" => "Constanta",
                "QuickStop Market" => "Brasov",
                "FamilyFood Express" => "Craiova",
                "FreshMart Downtown" => "Sibiu",
                "GreenGrocer Plaza" => "Oradea",
                _ => "Bucuresti"
            };
        }
    }
}