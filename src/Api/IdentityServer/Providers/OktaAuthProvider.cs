using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace IdentityServer.Providers
{
    public class OktaAuthProvider : BaseAuthProvider
    {
        private readonly AuthenticateResult _result;

        public OktaAuthProvider(AuthenticateResult result)
        {
            _result = result;
        }

        public override string GetEmail()
        {
            return _result.Principal.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault()?.Value;
        }

        public override string GetSubject()
        {
            return _result.Principal.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault()?.Value;
        }
    }
}
