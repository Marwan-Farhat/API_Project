using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Entities.Basket
{
    public class BasketItem
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }  // this price is the price of the product as an item in this basket (after Discount not original product price) 
        public int Quantity { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }

    }
}
