using Demo.APIs.Controllers.Base;
using Demo.Core.Domain.Contracts.Infrastructure;
using Demo.Shared.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.APIs.Controllers.Controllers
{
    [Authorize]
    public class PaymentController(IPaymentService paymentService) : BaseApiController
    {
        [Authorize]
        [HttpPost("{basketId}")] // POST: /api/payment/{basketId}
        public async Task<ActionResult<CustomerBasketDto>>CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);

            return Ok(json);
        }

    }
}
