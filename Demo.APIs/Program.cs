using Demo.Core.Application;
using Demo.APIs.Extensions;
using Demo.APIs.Services;
using Demo.Core.Application.Abstraction;
using Demo.Core.Domain.Contracts;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Demo.APIs.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using Demo.APIs.Middlewares;

namespace Demo.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
           
            builder.Services.AddControllers()       // Register Required services for Controllers by ASP.NET Core Web APIs To DI Container
                            .ConfigureApiBehaviorOptions(options=>
                            {
                                options.SuppressModelStateInvalidFilter = false;
                                options.InvalidModelStateResponseFactory = (actionContext) =>
                                {
                                    var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
                                   .SelectMany(P => P.Value!.Errors)
                                   .Select(E => E.ErrorMessage);

                                    return new BadRequestObjectResult(new ApiValidationErrorResponse()
                                    {
                                        Errors = errors
                                    });
                                };
                            }) 
                            .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly); // Make API Project to know that Controller in another project and give it Assembly Information of that project

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration); // Register DependencyInjection for DbContext
            builder.Services.AddApplicationServices();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));

            #endregion

            var app = builder.Build();

            #region Databases Initializer

            await app.InitializeStoreContextAsync();

            #endregion

            #region Configure Kestrel Middlwares

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.Run();

        }
    }
}
