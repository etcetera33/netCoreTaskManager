using Api.Configs;
using Api.Bus;
using Api.Middleware;
using AutoMapper;
using Data;
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
using Data.Interfaces;
using Data.Repositories;
using Core.Adapters;

namespace Api
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

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthConfig.GetKey(Configuration.GetSection("AuthConfig").GetSection("SecretKey").Value),
                    ValidateIssuer = false,
                    ValidateAudience = false
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

            services.AddSingleton<IHostedService, BusService>();
            // End of the service Bus

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IWorkItemRepository, WorkItemRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IWorkItemService, WorkItemService>();

            services.AddSingleton<IRedisService, RedisService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(AutoMapperConfiguration.Configure().CreateMapper());

            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            services.Configure<AuthConfig>(Configuration.GetSection("AuthConfig"));
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
                options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            redisService.Connect();

            app.UseMiddleware<RequestResponseLogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
