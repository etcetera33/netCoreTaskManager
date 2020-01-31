using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityServer4.Stores;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(
                //options => options.Events.RaiseSuccessEvents = true
                )
                .AddInMemoryCaching()
                .AddAspNetIdentity<IdentityUser>()
                .AddClientStore<InMemoryClientStore>()
                .AddResourceStore<InMemoryResourcesStore>();

            //    services.AddIdentityServer()
            //            .AddInMemoryIdentityResources(Configs.IdentityServerConfig.GetIdentityResources())
            //            .AddInMemoryApiResources(Configs.IdentityServerConfig.GetApiResources())
            //            .AddInMemoryClients(Configs.IdentityServerConfig.GetClients())
            //            .AddAspNetIdentity<IdentityUser>()
            //            .AddProfileService<Configs.IdentityProfileService>();

            //services.AddTransient<IEmailSender, AuthMessageSender>();


            services.AddAuthentication(options =>
            {
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = IdentityConstants.ExternalScheme;
                //options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            })
            .AddOpenIdConnect("Google", "Google",
                options =>
                {
                    options.SignInScheme = IdentityConstants.ExternalScheme;

                    IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google:OAuth2");
                    //options.SignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.SignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                    options.Authority = "https://accounts.google.com";
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.CallbackPath = "/signin-google";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                })
            .AddOpenIdConnect("oidc", "Azure AD", options =>
            {
                IConfigurationSection azureConfig = Configuration.GetSection("Authentication:AzureAd");

                options.Authority = "https://login.microsoftonline.com/common";
                options.ClientId = azureConfig["ClientId"];
                options.ClientSecret = azureConfig["ClientSecret"];
                
                options.CallbackPath = "/signin-oidc";

                options.ResponseType = OpenIdConnectResponseType.Code;

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

                options.Authority = oktaConfig["Domain"] + "/oauth2/default";
                options.RequireHttpsMetadata = true;
                options.ClientId = oktaConfig["ClientId"];
                options.ClientSecret = oktaConfig["ClientSecret"];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.SaveTokens = true;
                    
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };
            })
            ;

            services.AddControllersWithViews();
            services.AddRazorPages();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
