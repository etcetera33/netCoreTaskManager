using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4.Stores;
using IdentityServer.Configs;
using IdentityServer4;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Services;
using Data.Interfaces;
using Data.Repositories;
using Data;
using AutoMapper;
using Services.Mapper;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Cryptography;
using System.Text;
using System;
using IdentityServer.Extensions;

namespace IdentityServer
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
            services.AddDbContext<ApplicationDbContext>(
                       options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    );

            services.AddIdentityServer()
            .AddInMemoryCaching()
            .AddClientStore<InMemoryClientStore>()
            .AddResourceStore<InMemoryResourcesStore>()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            //.AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
            //.AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryClients(IIdentityServerHelper.AddInMemoryClientsWithClamis(Configuration.GetSection("IdentityServer:Clients")))
            .AddDeveloperSigningCredential()
            ;

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.DefaultSignOutScheme = IdentityServerConstants.SignoutScheme;
            })
           /*.AddGoogle("Google", "Google",
                options =>
                {
                    IConfigurationSection googleConfig = Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleConfig["ClientId"];
                    options.ClientSecret = googleConfig["ClientSecret"];
                    //options.Authority = "https://accounts.google.com";
                    //options.ResponseType = OpenIdConnectResponseType.Code;
                    options.CallbackPath = "/signin-google";
                    options.SaveTokens = true;
                })*/
            .AddOpenIdConnect("Google", "Google",
                options =>
                {
                    IConfigurationSection googleConfig = Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleConfig["ClientId"];
                    options.ClientSecret = googleConfig["ClientSecret"];
                    options.Authority = "https://accounts.google.com";
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.CallbackPath = "/signin-google";
                    options.SaveTokens = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    };
                })
            .AddOpenIdConnect("Azure", "Azure AD", options =>
            {
                IConfigurationSection azureConfig = Configuration.GetSection("Authentication:AzureAd");

                options.Authority = "https://login.microsoftonline.com/common";
                options.ClientId = azureConfig["ClientId"];
                options.ClientSecret = azureConfig["ClientSecret"];
                options.CallbackPath = "/signin-oidc";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SaveTokens = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidateIssuer = false
                };
            })
            .AddOpenIdConnect("Okta", "Okta", options => {
                IConfigurationSection oktaConfig = Configuration.GetSection("Authentication:Okta");

                options.CallbackPath = "/signin-okta";
                options.Authority = oktaConfig["Authority"];
                options.RequireHttpsMetadata = true;
                options.ClientId = oktaConfig["ClientId"];
                options.ClientSecret = oktaConfig["ClientSecret"];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.SaveTokens = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAutoMapper(typeof(Program));
            services.AddSingleton(AutoMapperConfiguration.Configure().CreateMapper());

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
