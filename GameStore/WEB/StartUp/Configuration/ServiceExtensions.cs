using GameStore.WEB.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            //AppSettings
            services.AddSingleton(appSettings);


        }
    }
}