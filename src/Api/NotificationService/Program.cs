using Core.Adapters;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        static async Task Main(string[] args)
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
                    services.Configure<RabbitmqConfig>(hostContext.Configuration.GetSection("RabbitmqConfig"));
                    services.Configure<MailConfig>(hostContext.Configuration.GetSection("MailConfig"));

                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddConsumer<EmailSendHandler>();

                        cfg.AddBus(ConfigureBus);
                    });

                    services.AddSingleton<IHostedService, BusService>();
                    services.AddSingleton<IHtmlContentBuilder, HtmlContentBuilder>();
                    services.AddSingleton<ISmtpClientProxy, SmtpClientProxy>();
                    services.AddSingleton<IMailer, Mailer>();

                    services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

                    services.AddRazorPages(options => options.RootDirectory = "/Views");
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            try
            {
                await builder.RunConsoleAsync();
                Log.Logger.Information("Started notofication service");
            }
            catch (Exception exception)
            {
                Log.Logger.Error(exception.Message);
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
