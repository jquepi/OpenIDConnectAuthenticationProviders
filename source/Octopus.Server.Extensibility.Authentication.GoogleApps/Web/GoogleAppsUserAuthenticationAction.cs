using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticationAction : UserAuthenticationAction<IGoogleAppsConfigurationStore>
    {
        public GoogleAppsUserAuthenticationAction(
            IGoogleAppsConfigurationStore configurationStore, 
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IGoogleAppsAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionResponseCreator responseCreator) : base(configurationStore, identityProviderConfigDiscoverer, urlBuilder, responseCreator)
        {
        }
    }
}