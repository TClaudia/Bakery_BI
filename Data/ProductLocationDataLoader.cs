using System;
using System.Collections.Generic;
using System.Linq;

namespace BakeryBI.Data
{
    public class ProductLocationDataLoader
    {
        public List<ProductLocation> GetProductLocations(List<SalesRecord> salesData)
        {
            if (salesData == null || !salesData.Any())
                return new List<ProductLocation>();

            var locations = salesData
                .GroupBy(r => r.StoreName)
                .Select(storeGroup =>
                {
                    var city = GetCityFromStoreName(storeGroup.Key);
                    var coords = StoreLocation.GetCityCoordinates(city);

                    // Grupăm pe produs și calculăm top 3
                    var topProducts = storeGroup
                        .GroupBy(r => r.ProductName)
                        .Select(productGroup => new
                        {
                            Product = productGroup.Key,
                            Sales = productGroup.Sum(r => r.FinalAmount),
                            Count = productGroup.Count()
                        })
                        .OrderByDescending(p => p.Sales)
                        .Take(3)
                        .ToList();

                    return new ProductLocation
                    {
                        StoreName = storeGroup.Key,
                        City = city,
                        Country = "Romania",
                        Latitude = coords.lat,
                        Longitude = coords.lon,
                        TopProduct = topProducts.ElementAtOrDefault(0)?.Product ?? "N/A",
                        TopProductSales = topProducts.ElementAtOrDefault(0)?.Sales ?? 0,
                        SecondProduct = topProducts.ElementAtOrDefault(1)?.Product ?? "N/A",
                        SecondProductSales = topProducts.ElementAtOrDefault(1)?.Sales ?? 0,
                        ThirdProduct = topProducts.ElementAtOrDefault(2)?.Product ?? "N/A",
                        ThirdProductSales = topProducts.ElementAtOrDefault(2)?.Sales ?? 0,
                        TotalTransactions = storeGroup.Count()
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

        // Metodă pentru a obține culoarea asociată unui produs
        public static string GetProductColor(string productName)
        {
            return productName switch
            {
                // Bakery Products - Tonuri de maro/auriu
                "Bread" => "#D2691E",           // Chocolate
                "Croissant" => "#F4A460",       // Sandy Brown
                "Pastry" => "#FFD700",          // Gold
                "Cookie" => "#CD853F",          // Peru

                // Dairy - Tonuri de alb/cream
                "Cheese" => "#FFF8DC",          // Cornsilk
                "Yogurt" => "#FFFACD",          // Lemon Chiffon
                "Milk" => "#F5F5DC",            // Beige

                // Meat/Protein - Tonuri de roșu/roz
                "Chicken Breast" => "#FF6B6B",  // Red
                "Salmon" => "#FF8C94",          // Light Red
                "Ground Beef" => "#DC143C",     // Crimson

                // Vegetables - Tonuri de verde
                "Bananas" => "#FFE135",         // Yellow
                "Apples" => "#FF4757",          // Apple Red
                "Onions" => "#DDA15E",          // Tan
                "Tomatoes" => "#FF6347",        // Tomato
                "Potatoes" => "#BC6C25",        // Brown
                "Carrots" => "#FF8C42",         // Orange

                // Grains - Tonuri de bej/maro deschis
                "Cereal" => "#E9C46A",          // Sandy
                "Rice" => "#F1FAEE",            // Mint Cream
                "Pasta" => "#FFDAB9",           // Peach

                // Beverages - Tonuri specifice
                "Coffee" => "#6F4E37",          // Coffee Brown
                "Tea" => "#C0E8C0",             // Tea Green
                "Orange Juice" => "#FFA500",    // Orange
                "Juice" => "#FF6F00",           // Dark Orange

                // Sweets - Tonuri de roz/violet
                "Cake" => "#FFB6C1",            // Light Pink
                "Muffin" => "#DDA0DD",          // Plum

                // Default
                _ => "#95A5A6"                  // Gray
            };
        }
    }
}