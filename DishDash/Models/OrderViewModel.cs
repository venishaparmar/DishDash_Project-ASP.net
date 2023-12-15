using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DishDash.Models
{
    public class OrderViewModel
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}