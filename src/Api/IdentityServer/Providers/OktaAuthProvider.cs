using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace IdentityServer.Providers
{
    public class OktaAuthProvider : IAuthProvider
    {
        private readonly AuthenticateResult _result;

        public OktaAuthProvider(AuthenticateResult result)
        {
            _result = result;
        }

        public string GetEmail()
        {
            return _result.Principal.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault()?.Value;
        }

        public string GetSubject()
        {
            return _result.Principal.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault()?.Value;
        }
    }
}
