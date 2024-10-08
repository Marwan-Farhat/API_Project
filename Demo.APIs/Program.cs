
using Demo.APIs.Extensions;
using Demo.APIs.Services;
using Demo.Core.Application.Abstraction;
using Demo.Core.Domain.Contracts;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
           
            builder.Services.AddControllers();   // Register Required services for Controllers by ASP.NET Core Web APIs To DI Container
        
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration); // Register DependencyInjection for DbContext

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));

            #endregion

            var app = builder.Build();

            #region Databases Initializer

            await app.InitializeStoreContextAsync();

            #endregion

            #region Configure Kestrel Middlwares

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers(); 

            #endregion

            app.Run();

        }
    }
}
