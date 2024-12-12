using Demo.Shared.Models.Basket;

namespace Demo.Core.Application.Abstraction.Services.Basket
{
    public interface IBasketService
    {
        Task<CustomerBasketDto?> GetCustomerBasketAsync(string basketId);
        Task<CustomerBasketDto?> UpdateCustomerBasketAsync(CustomerBasketDto basketDto);
        Task DeleteCustomerBasketAsync(string basketId);
    }
}
