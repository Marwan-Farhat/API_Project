using Demo.Core.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Contracts.Infrastructure
{
    public interface IBasketRepository
    {
        Task <CustomerBasket?> GetAsync (string id);
        Task <CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan timeToLive);
        Task<bool> DeleteAsync(string id);
    }
}
