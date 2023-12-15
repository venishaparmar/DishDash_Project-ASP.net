using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DishDash.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public byte[] image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityInCart { get; set; }
        public decimal finalAmount { get; set; }


        public ProductViewModel(Product product, int quantityInCart)
        {
            ProductId = product.id;
            Name = product.title;
            image = product.image;
            Price = product.price;
            QuantityInCart = quantityInCart;
            finalAmount = product.price * quantityInCart;
        }
    }

}