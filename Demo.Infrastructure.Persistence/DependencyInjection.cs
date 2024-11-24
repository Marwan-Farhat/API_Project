using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Contracts.Persistence.DbInitializers;
using Demo.Core.Domain.Identity;
using Demo.Infrastructure.Persistence._Identity;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Data.Interceptors;
using Demo.Infrastructure.Persistence.Identity;
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

            services.AddDbContext<StoreDbContext>((serviceProvider,optionsBuilder) =>
                {
                    optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("StoreContext"))
                    .AddInterceptors(serviceProvider.GetRequiredService<CustomSaveChangesInterceptor>());
                });

            services.AddScoped(typeof(IStoreDbInitializer), typeof(StoreDbInitializer));
            services.AddScoped(typeof(CustomSaveChangesInterceptor));
            // services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CustomSaveChangesInterceptor));

            #endregion

            #region Identity Context

            services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });

            services.AddScoped(typeof(IStoreIdentityDbInitializer), typeof(StoreIdentityDbInitializer));

            #endregion

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            // services.AddIdentityCore<ApplicationUser>();  // AddIdentityCore: Add Core Services Related with UserManager Only [UserManager]
                                                             // AddIdentity: Add All Services for Identity (We used it in MVC)   [UserManager, SignInManager, RoleManager]
            return services;
        }     
    }
}
