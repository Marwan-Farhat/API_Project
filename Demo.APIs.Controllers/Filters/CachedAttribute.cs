using Demo.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.APIs.Controllers.Filters
{
    internal class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var response = await responseCacheService.GetCachedResponseAsync(cacheKey);

            if(!string.IsNullOrEmpty(response))  // if response is already cached
            {
                var result = new ContentResult
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = result;

                return;
            }

            var executedActionContext = await next.Invoke();  // To Execute the endpoint if the  response is not cached

            if(executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }

        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            // {{url}}/api/products?pageIndex=1&pageSize=5&sort=name
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path);  // Path: api/products

            //pageIndex = 1
            //pageSize = 5
            //sort = name

            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))  // OrderBy cause a different request sent with the same values but in different order
            {
                keyBuilder.Append($"|{key}-{value}");
                // key = api/products|pageIndex-1
                // key = api/products|pageIndex-1|pageSize-5
                // key = api/products|pageIndex-1|pageSize-5sort-name
            }
            return keyBuilder.ToString();
        }
    }
}
