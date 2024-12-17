using AutoMapper;
using Demo.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using Demo.Core.Domain.Contracts.Infrastructure;
using Demo.Core.Domain.Entities.Basket;
using Demo.Shared.Exceptions;
using Demo.Shared.Models.Basket;
using Microsoft.Extensions.Configuration;

namespace Demo.Core.Application.Services.Basket
{
    internal class BasketService(IBasketRepository basketRepository, IMapper mapper,IConfiguration configuration) : IBasketService
    {
        public async Task<CustomerBasketDto?> GetCustomerBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetAsync(basketId);

            if (basket is null)
                throw new NotFoundException(nameof(CustomerBasket), basketId);

            return mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto?> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);

            var timeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!));

            var updatedBasket = await basketRepository.UpdateAsync(basket, timeToLive);

            if (updatedBasket is null)
                throw new BadRequestException("can't update, thir is a problem with your basket.");

            return basketDto;
        }
        public async Task DeleteCustomerBasketAsync(string basketId)
        {
            var deleted = await basketRepository.DeleteAsync(basketId);
            if (!deleted)
                throw new BadRequestException("unable to delete the basket");
        }
    }
}
