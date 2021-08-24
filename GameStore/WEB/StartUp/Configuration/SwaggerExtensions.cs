using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class SwaggerExtensions
    {
        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                c =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    c.SwaggerDoc("v1", new()
                    {
                        Version = "v1",
                        Title = "Click on 'TermsOfService' API",
                        Description = "Game store web API",
                        TermsOfService = new("../Home/GetInfo", UriKind.Relative)
                    });

                    c.CustomSchemaIds(x => x.FullName);
                    c.AddSecurityDefinition("Bearer", new()
                    {
                        Description = "JWT Authorization via Bearer scheme: Bearer {token}",
                        Scheme = "JWT",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                    c.AddSecurityRequirement(new()
                    {
                        {
                            new()
                            {
                                Reference = new()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });
        }

        public static void RegisterSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwagger(x => { x.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            app.UseSwaggerUI(o => { o.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore API V1"); });
        }
    }
}