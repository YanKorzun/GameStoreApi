using Microsoft.Extensions.DependencyInjection;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class AutoMapperExtensions
    {
        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
        }
    }
}