using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace IdentityServer.Providers
{
    public class GoogleAuthProvider : BaseAuthProvider
    {
        private readonly AuthenticateResult _result;

        public GoogleAuthProvider(AuthenticateResult result)
        {
            _result = result;
        }

        public override string GetEmail()
        {
            return _result.Principal.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault().Value;
        }

        public override string GetSubject()
        {
            return _result.Principal.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault()?.Value;
        }
    }
}
