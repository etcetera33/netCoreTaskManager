using IdentityServer4.Quickstart.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Models
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProviderConfig> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProviderConfig>();
        public IEnumerable<ExternalProviderConfig> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        public string Host { get; set; }
    }
}