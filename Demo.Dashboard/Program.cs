using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Identity;
using Demo.Dashboard.Helpers;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Identity;
using Demo.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Demo.Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<StoreDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("StoreContext"));
            });

            builder.Services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("IdentityContext"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
            {
                identityOptions.Password.RequireNonAlphanumeric = true;  // @#$%
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequireLowercase = true;              
            })
                            .AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MapsProfile));

            #endregion

            var app = builder.Build();

            #region Configure Kestrel Middlwares

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}"); 

            #endregion

            app.Run();
        }
    }
}
