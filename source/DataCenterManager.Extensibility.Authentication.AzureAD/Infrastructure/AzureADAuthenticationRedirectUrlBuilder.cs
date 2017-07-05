using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Issuer;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Infrastructure
{
    public class AzureADAuthenticationRedirectUrlBuilder : AuthenticationRedirectUrlBuilder<IAzureADConfigurationStore>
    {
        public AzureADAuthenticationRedirectUrlBuilder(
            ILog log,
            IAzureADConfigurationStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            IAzureADAuthorizationEndpointUrlBuilder urlBuilder, 
            IWebPortalConfigurationStore webPortalConfigurationStore) : 
            base(log, configurationStore, identityProviderConfigDiscoverer, urlBuilder, webPortalConfigurationStore)
        {
        }
    }
}