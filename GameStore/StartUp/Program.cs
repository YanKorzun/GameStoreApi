using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.RollingFileAlternative;

namespace GameStore.StartUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                       .MinimumLevel.Debug()
                       .WriteTo.Console()
                       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(@"Logs\Info-{Date}.log"))
                       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(@"Logs\Debug-{Date}.log"))
                       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(@"Logs\Warning-{Date}.log"))
                       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(@"Logs\Error-{Date}.log"))
                       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(@"Logs\Fatal-{Date}.log"))
                       .WriteTo.RollingFile(@"Logs\Verbose-{Date}.log")
                       .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
}