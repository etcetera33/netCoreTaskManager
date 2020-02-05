using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace IdentityServer.Providers
{
    public class GoogleAuthProvider : IAuthProvider
    {
        private readonly AuthenticateResult _result;

        public GoogleAuthProvider(AuthenticateResult result)
        {
            _result = result;
        }

        public string GetEmail()
        {
            return _result.Principal.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault().Value;
        }

        public string GetSubject()
        {
            return _result.Principal.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault()?.Value;
        }
    }
}
