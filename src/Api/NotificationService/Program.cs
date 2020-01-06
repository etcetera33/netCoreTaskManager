using MassTransit;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationService.Aggregates.HtmlAggregate;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Configs;
using NotificationService.Handlers;
using Serilog;
using System;
using System.Threading.Tasks;

namespace NotificationService
{
    class Program
    {
        private static RabbitmqConfig _appConfig;

        public static async Task Main(string[] args)
        {
               var builder = new HostBuilder()
                .UseSerilog()
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
                    services.Configure<RabbitmqConfig>(hostContext.Configuration.GetSection("RabbitmqConfig"));
                    services.Configure<MailConfig>(hostContext.Configuration.GetSection("MailConfig"));

                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddConsumer<EmailSendHandler>();

                        cfg.AddBus(ConfigureBus);
                    });

                    services.AddSingleton<IHostedService, BusService>();
                    services.AddSingleton<IHtmlContentBuilder, HtmlContentBuilder>();
                    services.AddSingleton<IHtmlBuilder, HtmlBuilder>();
                    services.AddSingleton<IMailer, Mailer>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(hostingContext.Configuration.GetSection("Serilog"))
                    .CreateLogger();
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
            _appConfig = provider.GetRequiredService<IOptions<RabbitmqConfig>>().Value;

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
