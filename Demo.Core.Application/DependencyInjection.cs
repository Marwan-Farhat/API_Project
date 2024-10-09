using Demo.Core.Application.Abstraction.Services;
using Demo.Core.Application.Abstraction.Services.Products;
using Demo.Core.Application.Mapping;
using Demo.Core.Application.Services;
using Demo.Core.Application.Services.Products;
using Demo.Core.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

            return services;
        }
    }
}
