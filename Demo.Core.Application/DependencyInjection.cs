using Demo.Core.Application.Abstraction.Services;
using Demo.Core.Application.Mapping;
using Demo.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Demo.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

            return services;
        }
    }
}
