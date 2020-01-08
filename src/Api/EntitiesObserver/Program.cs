using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EntitiesObserver.Handlers;
using Serilog;
using Services.Interfaces;
using Services;
using Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Mapper;
using EntitiesObserver.Configs;

namespace EntitiesObserver
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                //.UseSerilog()
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
                    services.AddDbContext<ApplicationDbContext>(
                       options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"))
                    );
                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddConsumer<WorkItemChangedHandler>();

                        cfg.AddBus(ConfigureBus);
                    });

                    services.AddSingleton<IHostedService, BusService>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IWorkItemService, WorkItemService>();


                    services.AddAutoMapper(typeof(Program));
                    services.AddSingleton(AutoMapperConfiguration.Configure().CreateMapper());
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
            

        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            var appConfig = provider.GetRequiredService<IOptions<AppConfig>>().Value;

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(appConfig.Host), h =>
                {
                    h.Username(appConfig.Username);
                    h.Password(appConfig.Password);
                });

                cfg.ConfigureEndpoints(provider);
            });

            return bus;
        }
    }
}
