using GameStore.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;

namespace GameStore.StartUp.Configuration
{
    public static class RegisterExceptionExtensions
    {
        public static void RegisterExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var exceptionResponse = new ExceptionResponse
                        {
                            Status = "Internal Server Error",
                            Message = contextFeature.Error.Message
                        };
                        Log.Error($"Error caught in global handler: '{exceptionResponse.Message}'");

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
    }
}