using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticationAction : UserAuthenticationAction<IGoogleAppsConfigurationStore>
    {
        public GoogleAppsUserAuthenticationAction(
            ILog log,
            IGoogleAppsConfigurationStore configurationStore, 
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IGoogleAppsAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionResponseCreator responseCreator,
            IApiActionModelBinder modelBinder,
            IWebPortalConfigurationStore webPortalConfigurationStore) 
                : base(log, configurationStore, identityProviderConfigDiscoverer, urlBuilder, responseCreator, modelBinder, webPortalConfigurationStore)
        {
        }
    }
}