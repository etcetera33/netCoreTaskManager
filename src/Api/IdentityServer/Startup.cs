using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4.Stores;
using IdentityServer.Configs;
using IdentityServer4;
using Services.Interfaces;
using Services;
using Data.Interfaces;
using Data.Repositories;
using Data;
using AutoMapper;
using Services.Mapper;
using IdentityServer.Extensions;
using Microsoft.IdentityModel.Logging;

namespace IdentityServer
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
            IdentityModelEventSource.ShowPII = true;

            services.AddDbContext<ApplicationDbContext>(
                       options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    );

            services.AddIdentityServer(options => {
                options.IssuerUri = Configuration.GetSection("Authorization").GetSection("Issuer").Value;
                options.PublicOrigin = Configuration.GetSection("Authorization").GetSection("Issuer").Value;
            })
            .AddInMemoryCaching()
            .AddClientStore<InMemoryClientStore>()
            .AddResourceStore<InMemoryResourcesStore>()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            .AddInMemoryClients(IIdentityServerExtensions.AddInMemoryClientsWithClamis(Configuration.GetSection("IdentityServer:Clients")))
            .AddDeveloperSigningCredential()
            ;

            services.AddAuthentication(options =>
            {
                 options.DefaultSignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                 options.DefaultSignOutScheme = IdentityServerConstants.SignoutScheme;
            })
            .AddOpenIdConnects(Configuration.GetSection("Authentication"));

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

            services.AddControllersWithViews(o =>
            {
                o.EnableEndpointRouting = false;
            });
            services.AddRazorPages();

            services.AddAutoMapper(typeof(Program));
            services.AddSingleton(AutoMapperConfiguration.Configure().CreateMapper());

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }    
            
            var basePath = Configuration.GetSection("Server").GetSection("BasePath").Value;
            if (!string.IsNullOrEmpty(basePath))
            {
                app.UsePathBase(basePath);
            }

            app.UseStaticFiles();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();
        
            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMvcWithDefaultRoute();
        }
    }
}
