using System;

using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Builder;
using GameStore.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.DAL;

namespace GameStore.StartUp.Configuration
{
    public static class HealthChecksExtensions
    {
        public static void RegisterHealthChecks(this IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
        }

        public static void RegisterHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health-check");
        }
    }
}
