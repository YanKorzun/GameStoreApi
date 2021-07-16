using GameStore.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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