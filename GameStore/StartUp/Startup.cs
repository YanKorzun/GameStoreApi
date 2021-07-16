using GameStore.Core.Configuration;
using GameStore.Startup.Configuration;
using GameStore.StartUp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace GameStore.StartUp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var AppSettings = RegisterSettings(Configuration);

            services.RegisterDatabase(AppSettings.Database);
            services.RegisterIdentity();
            services.RegisterAutoMapper();
            services.RegisterSwagger();
            services.RegisterHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<LoggingMiddleware>();
            app.RegisterSwaggerUi();
            app.RegisterHealthChecks();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.RegisterExceptionHandler(loggerFactory.CreateLogger("Exceptions"));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default", pattern: "{controller=HomeController}");
            });
        }

        private static AppSettings RegisterSettings(IConfiguration configuration) =>
                new()
                {
                    Database = configuration.GetSection(nameof(AppSettings.Database)).Get<DatabaseSettings>()
                };
    }
}