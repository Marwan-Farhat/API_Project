using Demo.Shared.Models.Orders;

namespace Demo.Core.Application.Abstraction.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail,OrderToCreateDto order);
        Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId);
        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();

    }
}
