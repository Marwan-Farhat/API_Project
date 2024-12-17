using AutoMapper;
using Demo.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using Demo.Core.Application.Abstraction.Services;
using Demo.Core.Application.Abstraction.Services.Orders;
using Demo.Core.Application.Mapping;
using Demo.Core.Application.Services;
using Demo.Core.Application.Services.Basket;
using Demo.Core.Application.Services.Orders;
using Demo.Core.Domain.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Demo.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));


            services.AddScoped(typeof(IBasketService), typeof(BasketService));
            //// Register the factory of Func<IBasketService>
            //services.AddScoped(typeof(Func<IBasketService>), (serviceProvider) =>
            //{
            //    return () => serviceProvider.GetRequiredService<IBasketService>();
            //});

            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            // Register the factory of Func<IBasketService>
            services.AddScoped(typeof(Func<IOrderService>), (serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IOrderService>();
            });

            return services;
        }
    }
}
