using System;

namespace BakeryBI.Data
{
    public class SalesRecord
    {
        public string CustomerId { get; set; }
        public string StoreName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Aisle { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public int LoyaltyPoints { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitMargin { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Quarter { get; set; }
        public string DayOfWeek { get; set; }
        public int WeekNumber { get; set; }
        public string CustomerType { get; set; }
        public string StoreCity { get; set; }
        public string StoreCountry { get; set; }
    }
}