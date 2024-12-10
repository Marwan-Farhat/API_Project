using Demo.Core.Domain.Entities.Basket;
using Demo.Shared.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Contracts.Infrastructure
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
    }
}
