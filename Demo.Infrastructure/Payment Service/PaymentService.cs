using Demo.Core.Application.Abstraction.Services.Basket;
using Demo.Core.Application.Abstraction.Services.Orders;
using Demo.Core.Application.Abstraction.Services.Products;
using Demo.Core.Application.Exceptions;
using Demo.Core.Domain.Contracts.Infrastructure;
using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Entities.Basket;
using Demo.Core.Domain.Entities.Orders;
using Demo.Core.Domain.Entities.Products;
using Demo.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Demo.Core.Domain.Entities.Products.Product;

namespace Demo.Infrastructure.Payment_Service
{
    internal class PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IOptions<RedisSettings> redisSettings) : IPaymentService
    {
        private readonly RedisSettings _redisSettings = redisSettings.Value;
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            // First Get Basket to Get the items
            var basket = await basketRepository.GetAsync(basketId);

            if (basket is null) throw new NotFoundException(nameof(CustomerBasket),basketId);

            // To Get Shipping Price throw Delivery Method
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);

                if (deliveryMethod is null) throw new NotFoundException(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);

                basket.ShippingPrice = deliveryMethod.Cost;
            }

            // To Validate that each item price is correct 
            if (basket.Items.Count > 0)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);

                    if (product is null) throw new NotFoundException(nameof(Product), item.Id);

                    if (item.Price != product.Price)
                          item.Price = product.Price;
                }
            }

            // Create or Update Payment Intent
            PaymentIntent? paymentIntent = null;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if(string.IsNullOrEmpty(basket.PaymentIntentId)) // Create New Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "Card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);  // Integration with stripe
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else  // Update an Existing Payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice * 100
                };

                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId,options);  // Integration with stripe

            }

            await basketRepository.UpdateAsync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays));

            return basket;
        }
    }
}
