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
            return $"<div class='text-center'><a href='{{{{authenticateLink}}}}'><img src=\"{siteBaseUri}/images/google_signin_buttons/btn_google_signin_light_normal_web.png\"/></a></div>";
        }
    }
}