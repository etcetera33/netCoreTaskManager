using Core.Adapters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.ErrorModels;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerAdapter<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerAdapter<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                if (ex is DbException || ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    await HandleError(ex.Message, "Data processing error", context);
                }
                else
                {
                    await HandleError(ex.Message, "Data processing error", context);
                }
            }
        }

        private Task HandleError(string errorMessage, string consumerMessage, HttpContext httpContext)
        {
            _logger.Error(errorMessage);

            var result = JsonConvert.SerializeObject(new ExceptionResponse{ Message = consumerMessage});
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            return httpContext.Response.WriteAsync(result);
        }
    }
}
