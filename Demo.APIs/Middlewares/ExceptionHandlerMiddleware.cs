﻿using Azure;
using Demo.APIs.Controllers.Errors;
using Demo.Core.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Demo.APIs.Controllers.Errors.ApiValidationErrorResponse;

namespace Demo.APIs.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionHandlerMiddleware(RequestDelegate next,ILogger<ExceptionHandlerMiddleware> logger,IWebHostEnvironment webHostEnvironment)
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
            catch (Exception ex)
            {
                #region Logging TODO

                if (_webHostEnvironment.IsDevelopment())
                {
                    // Development Mode
                    _logger.LogError(ex, ex.Message);
                }
                else
                {
                    // Production Mode
                    // Log Exception Details to Database || File(Text, json)
                }

                #endregion
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;

            switch (ex)
            {
                case NotFoundException:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(404, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case ValidationException:
                case BadRequestException:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(400, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case UnAuthorizedException:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(401, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                default:
                    response= _webHostEnvironment.IsDevelopment()? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                                                                 : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // == 500
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;
            }
        }
    }
}
