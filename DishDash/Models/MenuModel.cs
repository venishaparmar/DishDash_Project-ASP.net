using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DishDash.Models
{
    public class MenuModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductViewModel> AddCartProduct { get; set; }

    }
}