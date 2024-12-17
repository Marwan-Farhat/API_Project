using Demo.Core.Application.Abstraction.Common.Infrastructure;
using Demo.Core.Domain.Contracts.Infrastructure;
using Demo.Infrastructure.Basket_Repository;
using Demo.Infrastructure.Payment_Service;
using Demo.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure
{
    public static class DependencyInjection
    {  
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString!);
            });

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            return services;
        }
    }
}
