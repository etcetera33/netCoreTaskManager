using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EntitiesObserver.Handlers;
using EntitiesObserver.Interfaces;
using EntitiesObserver.Helpers;
using Microsoft.AspNetCore.Html;
using Serilog;

namespace EntitiesObserver
{
    public class Program
    {
        private static AppConfig _appConfig;

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));

                services.AddMassTransit(cfg =>
                {
                    cfg.AddConsumer<WorkItemChangedHandler>();

                    cfg.AddBus(ConfigureBus);
                });

                services.AddSingleton<IHostedService, BusService>();
                services.AddSingleton<IHtmlContentBuilder, HtmlContentBuilder>();
                services.AddSingleton<IHtmlBuilder, HtmlBuilder>();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });

            try
            {
                await builder.RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            _appConfig = provider.GetRequiredService<IOptions<AppConfig>>().Value;

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(_appConfig.Host), h =>
                {
                    h.Username(_appConfig.Username);
                    h.Password(_appConfig.Password);
                });

                cfg.ConfigureEndpoints(provider);
            });

            return bus;
        }
    }
}
