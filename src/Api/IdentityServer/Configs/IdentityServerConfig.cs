using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Configs
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "angular_spa",
                    ClientName = "Angular SPA",
                    ClientSecrets =
                    {
                        new Secret("angular_spa".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.read", "api1" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    ClientId = "jwt_client",
                    ClientSecrets =
                    {
                        new Secret("jwt_client".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "openid", "profile", "email", "api.read", "api1" },
                    Claims = { new Claim(ClaimTypes.Name, "10"), new Claim(ClaimTypes.Role, "Spectator") },
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
                {
                    Scopes = {new Scope("api.read")},
                    UserClaims = {"Name", "Role"}
                }
            };
        }
    }
}
