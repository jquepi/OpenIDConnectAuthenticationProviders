using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps
{
    public class GoogleAppsAuthenticationProvider : OpenIDConnectAuthenticationProvider<IGoogleAppsConfigurationStore>
    {
        public GoogleAppsAuthenticationProvider(ILog log, IGoogleAppsConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName => "Google Apps";

        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetIssuer()))
                yield return $"No {IdentityProviderName} issuer specified";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetHostedDomain()))
                yield return $"No {IdentityProviderName} hosted domain specified";
        }

        protected override string LoginLinkHtml(string absoluteVirtualDirectoryPath)
        {
            return "<google-auth-provider provider='provider' should-auto-login='shouldAutoLogin'></google-auth-provider>";
        }
    }
}