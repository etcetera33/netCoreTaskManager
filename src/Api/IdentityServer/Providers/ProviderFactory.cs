using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Providers
{
    public class ProviderFactory
    {
        public static BaseAuthProvider CreateProvider(AuthenticateResult result)
        {
            BaseAuthProvider provider;

            provider = (result.Properties.Items["scheme"]) switch
            {
                "Google" => new GoogleAuthProvider(result),

                "Azure" => new AzureAuthProvider(result),

                "Okta" => new OktaAuthProvider(result),

                _ => null,
            };

            return provider;
        }
    }
}
