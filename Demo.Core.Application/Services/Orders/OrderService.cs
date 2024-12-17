using AutoMapper;
using Demo.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using Demo.Core.Application.Abstraction.Common.Infrastructure;
using Demo.Core.Application.Abstraction.Services.Orders;
using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Entities.Orders;
using Demo.Core.Domain.Entities.Products;
using Demo.Core.Domain.Specifications.Orders;
using Demo.Shared.Exceptions;
using Demo.Shared.Models.Orders;

namespace Demo.Core.Application.Services.Orders
{
    internal class OrderService(IUnitOfWork unitOfWork,IMapper mapper,IBasketService basketService,IPaymentService paymentService) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order)
        {
            // 1.Get Basket From Basket Repo
            var basket = await basketService.GetCustomerBasketAsync(order.BasketId);

            // 2.Get Selected Items at Basket From Products Repo 
            var orderItems = new List<OrderItem>();

            if (basket!.Items.Count() >0)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);

                    if(product is not null)
                    {
                        var productItemOrdered = new ProductItemOrder()
                        {
                            Id = product.Id,
                            ProductName=product.Name,
                            PictureUrl = product.PictureUrl?? ""
                        };

                        var orderItem = new OrderItem()
                        { 
                            Product= productItemOrdered,
                            Price=product.Price,
                            Quantity=item.Quantity
                        };
                        orderItems.Add(orderItem);
                    }
                }
            }

            // 3.Calculate Subtotal
            var subTotal = orderItems.Sum(item=>item.Price * item.Quantity);

            // 4.Map Address
            var address = mapper.Map<Address>(order.ShippingAddress);

            // 5.Get Delivery Method
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(order.DeliveryMethodId);

            // 6. Check if there is order with the same paymentIntent (Like Lag in application customer press more than one time and the app make 2 orders with same  paymentIntentId)
            var orderRepo = unitOfWork.GetRepository<Order, int>();

            var orderSpecs = new OrderByPaymentIntentSpecifications(basket.PaymentIntentId!);

            var existingOrder = await orderRepo.GetWithSpecAsync(orderSpecs);

            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            // 7.Create Order
            var orderTOCreate = new Order()
            {
                BuyerEmail = buyerEmail,
                ShippingAddress=address,
                Items=orderItems,
                SubTotal=subTotal,
                DeliveryMethod= deliveryMethod,
                PaymentIntentId = basket.PaymentIntentId! 
            };
            await orderRepo.AddAsync(orderTOCreate);

            // 8.Save To Database
            var created = await unitOfWork.CompleteAsync() > 0;
            if (!created) throw new BadRequestException("an error occured during creating the order");

            return mapper.Map<OrderToReturnDto>(orderTOCreate);
        }
        public async Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            var orderSpecs = new OrderSpecifications(buyerEmail, orderId);
            var order = await unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(orderSpecs);

            if (order is null) throw new NotFoundException(nameof(Order),orderId);
            return mapper.Map<OrderToReturnDto>(order);

        }
        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderSpecs = new OrderSpecifications(buyerEmail);
            var orders = await unitOfWork.GetRepository<Order, int>().GetAllWithSpecAsync(orderSpecs);

            return mapper.Map< IEnumerable<OrderToReturnDto>>(orders);
        }
        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);

        }


    }
}
