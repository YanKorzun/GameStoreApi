using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GameStore.StartUp.Configuration
{
    public static class SwaggerExtensions
    {
        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Click on 'TermsOfService' API",
                        Description = "Lorem ipsum",
                        TermsOfService = new Uri("../Home/GetInfo", UriKind.Relative),

                    });
                });
        }

        public static void RegisterSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore API V1");
            });

        }
    }
}
