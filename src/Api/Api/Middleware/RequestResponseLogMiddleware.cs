using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class RequestResponseLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            dynamic data = new {};

            watch.Start();
            context.Response.OnStarting(() =>
            {
                watch.Stop();
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;

                Log.Logger.Information(
                    $"URL: {context.Request.Path} - Method {context.Request.Method} in {responseTimeForCompleteRequest} ms. Code: {context.Response.StatusCode}"
                );

                return Task.CompletedTask;
            });


            return this._next(context);
        }

    }
}
