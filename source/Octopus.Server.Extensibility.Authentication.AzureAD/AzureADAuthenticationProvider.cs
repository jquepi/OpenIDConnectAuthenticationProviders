using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.Server.Extensibility.Authentication.AzureAD
{
    public class AzureADAuthenticationProvider : OpenIDConnectAuthenticationProvider<IAzureADConfigurationStore>
    {
        public AzureADAuthenticationProvider(ILog log, IAzureADConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName => "Azure AD";

        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetIssuer()))
                yield return $"No {IdentityProviderName} issuer specified";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
        }

        protected override string LoginLinkHtml(string siteBaseUri)
        {
            return $"<a href='{{{{authenticateLink}}}}'><div class=\"external-provider-button aad-button\"><img src=\"{siteBaseUri}/images/microsoft_signin_buttons/microsoft-logo.svg\"><div>Sign in with Microsoft</div></div></a>";
        }
    }
}