using Api.Bus;
using Api.Middleware;
using AutoMapper;
using Core.Adapters;
using Core.Configs;
using Core.Security;
using Data;
using Data.Interfaces;
using Data.Repositories;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Serilog;
using Services;
using Services.Helpers;
using Services.Interfaces;
using Services.Mapper;
using Services.Validators;

namespace Api
{
    public class Startup
    {
        private readonly ILoggerAdapter<Startup> _logger;

        public Startup(IConfiguration configuration, ILoggerAdapter<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>())
                ;

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            _logger.Information("Connection string:" + Configuration.GetConnectionString("DefaultConnection"));
            //_logger.Information("Connection string:" + Configuration.GetConnectionString("DefaultConnection"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.Authority = Configuration.GetSection("IdentityServer").GetSection("Host").Value;
                x.Audience = "api1";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = TaskManagerClaims.Name,
                    RoleClaimType = TaskManagerClaims.Role
                };
            });

            // Start of the service Bus1
            services.AddMassTransit();

            services.AddSingleton(provider => MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(
                    host: Configuration.GetSection("RabbitMqConfig").GetSection("Host").Value,
                    virtualHost: Configuration.GetSection("RabbitMqConfig").GetSection("VirtualHost").Value,
                    h => { });

                cfg.ReceiveEndpoint(
                    Configuration.GetSection("RabbitMqConfig").GetSection("Endpoint").Value,
                    e => { });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddTransient<IFileUploader, FileUploader>();

            services.AddSingleton<IHostedService, BusService>();
            // End of the service Bus

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IWorkItemRepository, WorkItemRepository>();
            services.AddTransient<IWorkItemAuditRepository, WorkItemAuditRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IWorkItemFileRepository, WorkItemFileRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IWorkItemService, WorkItemService>();
            services.AddTransient<IWorkItemAuditService, WorkItemAuditService>();
            services.AddTransient<IFileService, FileService>();

            services.AddSingleton<IRedisService, RedisService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(AutoMapperConfiguration.Configure().CreateMapper());

            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            services.Configure<AuthConfig>(Configuration.GetSection("AuthConfig"));
            services.Configure<AzureConfig>(Configuration.GetSection("AzureConfig"));
            services.Configure<RedisConfig>(Configuration.GetSection("Redis"));
            services.Configure<PasswordHasher>(Configuration.GetSection("PasswordHash"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRedisService redisService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            redisService.Connect();

            app.UseMiddleware<RequestResponseLogMiddleware>();

            app.UseMiddleware<ErrorLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
