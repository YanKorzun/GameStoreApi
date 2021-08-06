using GameStore.Startup.Configuration;
using GameStore.WEB.Settings;
using GameStore.WEB.Startup.Configuration;
using GameStore.WEB.StartUp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var AppSettings = RegisterSettings(Configuration);
            services.RegisterServices(AppSettings);
            services.RegisterDatabase(AppSettings.Database, LoggerFactory);
            services.RegisterAutoMapper();
            services.RegisterSwagger();
            services.RegisterHealthChecks();
            services.RegisterIdentity();
            services.RegisterAuthSettings(AppSettings.Token);
            services.RegisterHttpContextExtensions();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<LoggingMiddleware>();
            app.RegisterSwaggerUi();
            app.RegisterHealthChecks();
            app.UseHttpsRedirection();
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
                    SmtpClientSettings = configuration.GetSection(nameof(AppSettings.SmtpClientSettings)).Get<SmtpClientSettings>(),
                    CloudinarySettings = configuration.GetSection(nameof(AppSettings.CloudinarySettings)).Get<CloudinarySettings>()
                };
    }
}