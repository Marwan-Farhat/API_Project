using Demo.Shared.Models.Basket;

namespace Demo.Core.Application.Abstraction.Common.Infrastructure
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
        Task UpdateOrderPaymentStatus(string requestBody, string header);
    }
}
