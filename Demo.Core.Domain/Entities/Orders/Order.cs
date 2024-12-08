using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Entities.Orders
{
    public class Order:BaseAuditableEntity<int>
    {
        public required string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.pending;
        public required Address ShippingAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod!.Cost;
        public string PaymentIntentId { get; set; } = "";


        // Navigational Properties Foreign Keys
        public int? DeliveryMethodId { get; set; }

        // Navigational Properties
        public  virtual DeliveryMethod? DeliveryMethod { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }=new HashSet<OrderItem>();

    }
}
