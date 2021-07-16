using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace GameStore.Startup.Configuration
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var jsonBody = string.Empty;

                if (context.Request != null && context.Request.Body != null)
                {
                    context.Request.EnableBuffering();
                    jsonBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                Log.Information($"Request {context.Request?.Method} {context.Request?.Path.Value} {jsonBody} => {context.Response?.StatusCode}");
            }
            finally
            {
                await _next(context);
            }
        }
    }
}