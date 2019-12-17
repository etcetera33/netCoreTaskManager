using Api.Auth;
using Api.Middleware;
using AutoMapper;
using Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Models.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Services;
using Services.Helpers;
using Services.Interfaces;
using Services.Mapper;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            
            // configure jwt authentication
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
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = false,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = false
                };
            });

            // DI
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IWorkItemService, WorkItemService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            // mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(AutoMapperConfiguration.Configure().CreateMapper());

            // cfgs
            services.Configure<AuthConfig>(Configuration.GetSection("AuthConfig"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
            );

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseMiddleware<RequestResponseLogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
