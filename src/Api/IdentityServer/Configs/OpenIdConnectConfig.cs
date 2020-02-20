using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace IdentityServer.Configs
{
    public class OpenIdConnectConfig : OpenIdConnectOptions
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
        public bool ValidateIssuer { get; set; }
    }
}
