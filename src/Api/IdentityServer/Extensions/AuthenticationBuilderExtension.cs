using IdentityServer.Configs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace IdentityServer.Extensions
{
    public static class AuthenticationBuilderExtension
    {
        public static AuthenticationBuilder AddOpenIdConnects(this AuthenticationBuilder authenticationBuilder, IConfigurationSection configurationSection)
        {
            var openIdConnections = configurationSection.Get<OpenIdConnectConfig[]>();

            if (openIdConnections == null)
            {
                return authenticationBuilder;
            }

            foreach (var connect in openIdConnections)
            {
                authenticationBuilder.AddOpenIdConnect(connect.AuthenticationScheme, connect.DisplayName, options =>
                {
                    options.ClientId = connect.ClientId;
                    options.ClientSecret = connect.ClientSecret;
                    options.Authority = connect.Authority;
                    options.ResponseType = connect.ResponseType;
                    options.CallbackPath = connect.CallbackPath;
                    options.SaveTokens = connect.SaveTokens;

                    foreach(var scope in connect.Scope.ToList())
                    {
                        options.Scope.Add(scope);
                    }
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = connect.NameClaimType,
                        RoleClaimType = connect.RoleClaimType,
                        ValidateIssuer = connect.ValidateIssuer
                    };
                });
            }

            return authenticationBuilder;
        }
    }
}
