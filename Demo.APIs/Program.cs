
namespace Demo.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
           
            builder.Services.AddControllers();   // Register Required services for Controllers by ASP.NET Core Web APIs To DI Container
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 

            #endregion  

            var app = builder.Build();

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
