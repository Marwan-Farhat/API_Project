using Demo.Core.Domain.Contracts;

namespace Demo.APIs.Extensions
{
    public static class InitializerExtensionsClass
    {
        public static async Task<WebApplication> InitializeStoreContextAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();             // To Create a scoped Request Explicitly
            var services = scope.ServiceProvider;                          // ServiceProvider method provide for me scoped services to choose 
            var storeContextInitializer = services.GetRequiredService<IStoreContextInitializer>();   // CLR Create object from StoreContext

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();  // To Log Exceptions

            try
            {
                await storeContextInitializer.InitializeAsync();      // To Update Database
                await storeContextInitializer.SeedAsync();            // To Seed Data
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occured during applying the migration");
            }

            return app;
        }
    }
}
