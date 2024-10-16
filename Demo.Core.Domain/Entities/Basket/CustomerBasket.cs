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
        public IEnumerable<BasketItem> Items { get; set; }=new List<BasketItem>();
    }
}
