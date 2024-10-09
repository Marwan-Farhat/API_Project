using Demo.Core.Application.Abstraction;
using Demo.Core.Domain.Contracts;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreContext>((optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("StoreContext"));
            });

            services.AddScoped<IStoreContextInitializer, StoreContextInitializer>();
            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CustomSaveChangesInterceptor));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            return services;
        }     
    }
}
