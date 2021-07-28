using GameStore.BL.Interfaces;
using GameStore.BL.Services;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSingleton(appSettings);

            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}