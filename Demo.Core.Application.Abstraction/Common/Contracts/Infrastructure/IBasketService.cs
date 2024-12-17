using Demo.Shared.Models.Basket;

namespace Demo.Core.Application.Abstraction.Common.Contracts.Infrastructure
{
    public interface IBasketService
    {
        Task<CustomerBasketDto?> GetCustomerBasketAsync(string basketId);
        Task<CustomerBasketDto?> UpdateCustomerBasketAsync(CustomerBasketDto basketDto);
        Task DeleteCustomerBasketAsync(string basketId);
    }
}
