using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps
{
    public class GoogleAppsAuthenticationProvider : OpenIDConnectAuthenticationProvider<IGoogleAppsConfigurationStore>
    {
        public GoogleAppsAuthenticationProvider(IGoogleAppsConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public override string IdentityProviderName => "Google Apps";

        protected override bool IsProviderConfigComplete()
        {
            return !string.IsNullOrWhiteSpace(ConfigurationStore.GetIssuer()) &&
                !string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()) &&
                !string.IsNullOrWhiteSpace(ConfigurationStore.GetHostedDomain());
        }

        protected override string LoginLinkHtml(string siteBaseUri)
        {
            return $"<a href='{{{{authenticateLink}}}}'><div class=\"external-provider-button googleapps-button\"><img src=\"{siteBaseUri}/images/google_signin_buttons/icon-google.svg\"><div>Sign in with Google</div></div></a>";
        }
    }
}