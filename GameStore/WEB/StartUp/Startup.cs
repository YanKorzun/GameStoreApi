using GameStore.WEB.Settings;
using GameStore.WEB.Startup.Configuration;
using GameStore.WEB.StartUp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameStore.WEB.StartUp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }
        private ILoggerFactory LoggerFactory { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = RegisterSettings(Configuration);
            services.RegisterServices(appSettings);
            services.RegisterDatabase(appSettings.Database, LoggerFactory);
            services.RegisterAutoMapper();
            services.RegisterSwagger();
            services.RegisterHealthChecks();
            services.RegisterIdentity();
            services.RegisterAuthSettings(appSettings.Token);
            services.RegisterHttpContextExtensions();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<LoggingMiddleware>();
            app.RegisterSwaggerUi();
            app.RegisterHealthChecks();
            app.UseStaticFiles();
            app.RegisterExceptionHandler(loggerFactory.CreateLogger("Exceptions"));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static AppSettings RegisterSettings(IConfiguration configuration) =>
            new()
            {
                Database = configuration.GetSection(nameof(AppSettings.Database)).Get<DatabaseSettings>(),
                Token = configuration.GetSection(nameof(AppSettings.Token)).Get<TokenSettings>(),
                SmtpClientSettings = configuration.GetSection(nameof(AppSettings.SmtpClientSettings))
                    .Get<SmtpClientSettings>(),
                CloudinarySettings = configuration.GetSection(nameof(AppSettings.CloudinarySettings))
                    .Get<CloudinarySettings>()
            };
    }
}