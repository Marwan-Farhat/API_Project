
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration); // Register DependencyInjection for DbContext

            #endregion

            var app = builder.Build();

            using var scope = app.Services.CreateAsyncScope();             // To Create a scoped Request Explicitly
            var services = scope.ServiceProvider;                          // ServiceProvider method provide for me scoped services to choose 
            var dbContext = services.GetRequiredService<StoreContext>();   // CLR Create object from StoreContext

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();  // To Log Exceptions

            try
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();  // Will Send a request to get if there is any pending Migrations
                if (pendingMigrations.Any())
                    await dbContext.Database.MigrateAsync();            // Update Database
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occured during applying the migration");
            }

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
