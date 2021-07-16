using GameStore.Core.Configuration;
using GameStore.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.StartUp.Configuration
{
    public static class DatabaseExtensions
    {
        public static void RegisterDatabase(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(databaseSettings.ConnectionString);
                }, ServiceLifetime.Transient

            );
            services.AddDatabaseDeveloperPageExceptionFilter();
        }
    }
}