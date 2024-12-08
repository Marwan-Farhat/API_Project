using Demo.Infrastructure.Persistence.Identity;
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
using Demo.Infrastructure;
using Demo.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Demo.Core.Domain.Contracts.Persistence.DbInitializers;
using Newtonsoft.Json;
namespace Demo.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            builder.Services.AddControllers()     // Register Required services for Controllers by ASP.NET Core Web APIs To DI Container
                            .AddNewtonsoftJson(options =>
                            { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; })
                            .ConfigureApiBehaviorOptions(options=>
                            {
                                options.SuppressModelStateInvalidFilter = false;
                                options.InvalidModelStateResponseFactory = (actionContext) =>
                                {
                                    var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
                                                                         .Select(P=> new ApiValidationErrorResponse.ValidationError()
                                                                         {
                                                                             Field=P.Key,
                                                                             Errors=P.Value!.Errors.Select(E=>E.ErrorMessage)
                                                                         });

                                    return new BadRequestObjectResult(new ApiValidationErrorResponse()
                                    {
                                        Errors = errors
                                    });
                                };
                            }) 
                            .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly); // Make API Project to know that Controller in another project and give it Assembly Information of that project

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration); // Register DependencyInjection for Persistence Layer (DbContext)
            builder.Services.AddApplicationServices();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddIdentityServices(builder.Configuration);

            #endregion

            var app = builder.Build();

            #region Databases Initializer

            await app.InitializeDbAsync();

            #endregion

            #region Configure Kestrel Middlewares

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();

        }
    }
}
