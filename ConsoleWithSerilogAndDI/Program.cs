using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleWithSerilogAndDI
{
    // IAmTimCorey
    // .NET Core Console App with Dependency Injection, Logging, and Settings
    // https://www.youtube.com/watch?v=GAOCe-2nXqc
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = GetConfigurationBuilder();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("The application has started");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // DI Registrations
                    services.AddTransient<IGreetingService, GreetingService>();
                })
                .UseSerilog()
                .Build();

            var svc = host.Services.GetService<IGreetingService>();
            await svc.RunAsync();
        }

        static IConfigurationBuilder GetConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPENTCORE_ENVIRONMENT") ?? Environments.Production}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder;
        }
    }
}
