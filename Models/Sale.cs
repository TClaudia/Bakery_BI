using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBI.Models
{
        public class Sale

        {
            public string customer_id { get; set; }
            public string store_name { get; set; }
            public DateTime transaction_date { get; set; }
            public string aisle { get; set; }
            public string product_name { get; set; }
            public int quantity { get; set; }
            public decimal unit_price { get; set; }
            public decimal total_amount { get; set; }
            public decimal discount_amount { get; set; }
            public decimal final_amount { get; set; }
            public int loyalty_points { get; set; }
            public decimal unit_cost { get; set; }
            public decimal total_cost { get; set; }
            public decimal profit { get; set; }
            public decimal profit_margin { get; set; }
            public int year { get; set; }
            public int month { get; set; }
            public string month_name { get; set; }
            public int quarter { get; set; }
            public string day_of_week { get; set; }
            public int week_number { get; set; }
            public string customer_type { get; set; }
        }
    }
