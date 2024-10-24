using Demo.Core.Domain.Contracts.Persistence.DbInitializers;

namespace Demo.APIs.Extensions
{
    public static class InitializerExtensionsClass
    {
        public static async Task<WebApplication> InitializeStoreContextAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();             // To Create a scoped Request Explicitly
            var services = scope.ServiceProvider;                          // ServiceProvider method provide for me scoped services to choose 


            var storeContextInitializer = services.GetRequiredService<IStoreDbInitializer>();   // CLR Create object from StoreContext
            var identityContextInitializer = services.GetRequiredService<IStoreIdentityDbInitializer>();   // CLR Create object from StoreIdentityDbContext

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();  // To Log Exceptions

            try
            {
                await storeContextInitializer.InitializeAsync();      // To Update Database for storeContext
                await storeContextInitializer.SeedAsync();            // To Seed Data for storeContext

                await identityContextInitializer.InitializeAsync();      // To Update Database for identityContext
                await identityContextInitializer.SeedAsync();            // To Seed Data for identityContext
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
