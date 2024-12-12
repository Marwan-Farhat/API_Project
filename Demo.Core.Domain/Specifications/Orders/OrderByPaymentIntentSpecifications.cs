using Demo.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Specifications.Orders
{
    public class OrderByPaymentIntentSpecifications:BaseSpecifications<Order,int>
    {
        public OrderByPaymentIntentSpecifications(string paymentIntentId)
            : base(order => order.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
