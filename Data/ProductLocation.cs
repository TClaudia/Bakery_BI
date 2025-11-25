using System;

namespace BakeryBI.Data
{
    public class ProductLocation
    {
        public string StoreName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TopProduct { get; set; } = string.Empty;
        public decimal TopProductSales { get; set; }
        public string SecondProduct { get; set; } = string.Empty;
        public decimal SecondProductSales { get; set; }
        public string ThirdProduct { get; set; } = string.Empty;
        public decimal ThirdProductSales { get; set; }
        public int TotalTransactions { get; set; }
    }
}