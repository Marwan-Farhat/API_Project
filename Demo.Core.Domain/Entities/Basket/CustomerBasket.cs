using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Entities.Basket
{
    public class CustomerBasket
    {
        public required string Id { get; set; }
        public ICollection<BasketItem> Items { get; set; }=new HashSet<BasketItem>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
