using Demo.Core.Domain.Identity;
using Demo.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace Demo.APIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
            {
                identityOptions.User.RequireUniqueEmail = true;
                // identityOptions.User.AllowedUserNameCharacters = "dcs$#FCD";

                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

                identityOptions.Password.RequireNonAlphanumeric = true;  // @#$%
                identityOptions.Password.RequiredUniqueChars = 2;
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequireLowercase = true;

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 10;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                // identityOptions.Stores
                // identityOptions.ClaimsIdentity
                // identityOptions.Tokens
            })
                        .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }
    }
}
