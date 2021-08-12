using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class RegisterExceptionExtensions
    {
        public static void RegisterExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                        if (contextFeature is not null)
                        {
                            var exceptionResponse = new ExceptionResponse
                            {
                                Status = "Internal Server Error",
                                Message = contextFeature.Error.Message
                            };
                            logger.LogError($"Error caught in global handler: '{exceptionResponse.Message}'");

                            await context.Response.WriteAsync($@"
                            {{
                                ""errors"": {{
                                    ""code"":""API_server_error"",
                                    ""status"": ""{exceptionResponse.Status}"",
                                    ""message"":""{exceptionResponse.Message}""
                                }}
                            }}
                        ");
                        }
                    });
                }
            );
        }

        private record ExceptionResponse
        {
            public string Status { get; init; }
            public string Message { get; init; }
        }
    }
}