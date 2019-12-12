using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
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
            
            watch.Start();
            context.Response.OnStarting(() =>
            {
                watch.Stop();
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;

                var data = new
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Time = responseTimeForCompleteRequest
                };

                Log.Logger.Information(
                        GetLoggerMessage(data)
                    );
             
                return Task.CompletedTask;
            });  

            return this._next(context);
        }

        private string GetLoggerMessage(dynamic data)
        {
            return $"Method: {data.Method}. URL: {data.Path}. The time spent on the request: {data.Time} ms.";
        }
    }
}
