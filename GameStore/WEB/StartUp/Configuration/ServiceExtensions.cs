using GameStore.BL.Aggregators;
using GameStore.BL.Interfaces;
using GameStore.BL.Services;
using GameStore.BL.Utilities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.WEB.Filters.ActionFilters;
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
            //Services
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductLibraryService, ProductLibraryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProductRatingService, ProductRatingService>();
            //Utilities
            services.AddTransient<IClaimsUtility, ClaimsUtility>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            //Aggregators
            services.AddTransient<ICustomProductAggregator, ProductAggregator>();
            //Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductRatingRepository, ProductRatingRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProductLibraryRepository, ProductLibraryRepository>();
            //Action filters
            services.AddScoped<ValidationFilterAttribute>();
        }
    }
}