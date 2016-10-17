using System.Collections.Generic;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.HostServices.Diagnostics;

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

        protected override string LoginLinkHtml(string siteBaseUri)
        {
            return $"<a href='{{{{authenticateLink}}}}'><div class=\"external-provider-button googleapps-button\"><img src=\"{siteBaseUri}/images/google_signin_buttons/icon-google.svg\"><div>Sign in with Google</div></div></a>";
        }
    }
}