using Demo.Core.Domain.Contracts.Persistence;
using Demo.Infrastructure.Persistence._Identity;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Store Context

            services.AddDbContext<StoreDbContext>((optionsBuilder) =>
                {
                    optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("StoreContext"));
                });

            services.AddScoped<IStoreContextInitializer, StoreDbInitializer>();
            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CustomSaveChangesInterceptor));

            #endregion

            #region Identity Context

            services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });

            #endregion

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            return services;
        }     
    }
}
