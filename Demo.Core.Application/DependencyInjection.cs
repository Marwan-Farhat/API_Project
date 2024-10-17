using AutoMapper;
using Demo.Core.Application.Abstraction.Services;
using Demo.Core.Application.Abstraction.Services.Basket;
using Demo.Core.Application.Mapping;
using Demo.Core.Application.Services;
using Demo.Core.Application.Services.Basket;
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

            /// services.AddScoped(typeof(IBasketService), typeof(BasketService));
            /// services.AddScoped(typeof(Func<IBasketService>), typeof(Func<BasketService>));
            services.AddScoped(typeof(Func<IBasketService>), (serviceProvider) =>
            {
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var basketRepository = serviceProvider.GetRequiredService<IBasketRepository>();

                return () => new BasketService(basketRepository, mapper, configuration);
            });

            return services;
        }
    }
}
