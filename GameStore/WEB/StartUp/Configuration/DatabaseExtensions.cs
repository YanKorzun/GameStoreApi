using GameStore.DAL;
using GameStore.WEB.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class DatabaseExtensions
    {
        public static void RegisterDatabase(this IServiceCollection services, DatabaseSettings databaseSettings, ILoggerFactory loggerFactory)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(databaseSettings.ConnectionString);
                    options.UseLoggerFactory(loggerFactory);
                }, ServiceLifetime.Transient

            );
            services.AddDatabaseDeveloperPageExceptionFilter();
        }
    }
}