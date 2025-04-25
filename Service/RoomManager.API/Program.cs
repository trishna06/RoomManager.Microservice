using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RoomManager.API.Configurations;
using RoomManager.Application.Helpers;
using Serilog;

namespace RoomManager.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            Log.Information("Configuring web host ({ApplicationContext})...", ApplicationHelper.ApplicationName);
            Log.Information("Starting up!");

            try
            {
                CreateHostBuilder(args).Build().Run();

                Log.Information("Stopped cleanly");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.GetConfigurationBuilder();
                })
                .UseSerilog((hostingContext, configuration) =>
                {
                    configuration.GetLoggerConfiguration(hostingContext.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }

    public static class ProgramExtensions
    {
        public static IConfigurationBuilder GetConfigurationBuilder(this IConfigurationBuilder configuration)
        {
            return configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        public static LoggerConfiguration GetLoggerConfiguration(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {

            loggerConfiguration
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(configuration.GetSection(nameof(LogOptions)))
                .Enrich.FromLogContext()
                .WriteTo.Console();

            return loggerConfiguration;
        }
    }
}
