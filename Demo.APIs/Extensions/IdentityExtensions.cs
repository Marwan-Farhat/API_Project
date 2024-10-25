using Demo.Core.Application.Abstraction.Models.Auth;
using Demo.Core.Application.Abstraction.Services.Auth;
using Demo.Core.Application.Services.Auth;
using Demo.Core.Domain.Identity;
using Demo.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Demo.APIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));

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

            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(Func<IAuthService>), (serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IAuthService>();
            });

            services.AddAuthentication((authenticationOptions) =>
            { 
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // JwtBearerDefaults.AuthenticationScheme == Bearer
                // authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer((configurationOptions) =>
                {
                    configurationOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience=true,
                        ValidateIssuer=true,
                        ValidateIssuerSigningKey=true,
                        ValidateLifetime=true,

                        ClockSkew=TimeSpan.FromMinutes(0),
                        ValidIssuer = configuration["jwtSettings:Issuer"],
                        ValidAudience = configuration["jwtSettings:Audience"],
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtSettings:Key"]!))
                    };
                });

            return services;
        }
    }
}
