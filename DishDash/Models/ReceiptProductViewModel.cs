using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DishDash.Models
{
    public class ReceiptProductViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}