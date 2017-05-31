using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsHomeLinksContributor : OpenIDConnectHomeLinksContributor<IGoogleAppsConfigurationStore, GoogleAppsAuthenticationProvider>
    {
        public GoogleAppsHomeLinksContributor(IGoogleAppsConfigurationStore configurationStore, GoogleAppsAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}