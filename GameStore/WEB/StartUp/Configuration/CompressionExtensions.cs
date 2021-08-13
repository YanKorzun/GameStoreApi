using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.WEB.StartUp.Configuration
{
    public static class CompressionExtensions
    {
        public static void RegisterCompression(this IServiceCollection services)
        {
            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        public static void RegisterCompression(this IApplicationBuilder app)
        {
            app.UseResponseCompression();
        }
    }
}