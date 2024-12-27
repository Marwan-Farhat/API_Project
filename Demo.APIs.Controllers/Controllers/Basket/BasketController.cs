using Demo.APIs.Controllers.Base;
using Demo.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using Demo.Shared.Models.Basket;
using Microsoft.AspNetCore.Mvc;

namespace Demo.APIs.Controllers.Controllers.Basket
{
    public class BasketController(IBasketService basketService) : BaseApiController
    {
        [HttpGet]  // Get: /api/Basket?id
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
        {
            var basket = await basketService.GetCustomerBasketAsync(id);
            return Ok(basket);
        }


        [HttpPost]  // Post: /api/Basket
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = await basketService.UpdateCustomerBasketAsync(basketDto);
            return Ok(basket);
        }


        [HttpDelete]  // Delete: /api/Basket
        public async Task DeleteBasket(string id)
        {
            await basketService.DeleteCustomerBasketAsync(id);
        }
    }
}
