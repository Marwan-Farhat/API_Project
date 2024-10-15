using Azure;
using Demo.APIs.Controllers.Errors;
using Demo.APIs.Controllers.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Demo.APIs.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,ILogger<CustomExceptionHandlerMiddleware> logger,IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        } 
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try {
                // Logic Executed with the Request
                await _next(httpContext);
                // Logic Executed with the Response
            }
            catch (Exception ex) {

                ApiResponse response;

                switch (ex)
                {
                    case NotFoundException:

                        httpContext.Response.StatusCode= (int)HttpStatusCode.NotFound;
                        httpContext.Response.ContentType = "application/json";

                         response = new ApiResponse(404, ex.Message);
                        await httpContext.Response.WriteAsync(response.ToString());
                        break;
                    default:


                        if (_webHostEnvironment.IsDevelopment())
                        {
                            // Development Mode
                            _logger.LogError(ex, ex.Message);
                            response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString());
                        }
                        else
                        {
                            // Production Mode
                            // Log Exception Details to Database || File(Text, json)
                            response = new ApiExceptionResponse(500);
                        }

                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // == 500
                        httpContext.Response.ContentType = "application/json";
                        await httpContext.Response.WriteAsync(response.ToString());
                        break;
                }
            }
        }
    }
}
