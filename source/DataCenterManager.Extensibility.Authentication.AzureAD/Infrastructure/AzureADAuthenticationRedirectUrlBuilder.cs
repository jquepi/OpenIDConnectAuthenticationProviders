using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Infrastructure
{
    public class AzureADAuthenticationRedirectUrlBuilder : AuthenticationRedirectUrlBuilder<AzureADConfigurationStore>
    {
        public AzureADAuthenticationRedirectUrlBuilder(
            ILog log,
            AzureADConfigurationStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IAuthorizationEndpointUrlBuilder urlBuilder, 
            IWebPortalConfigurationStore webPortalConfigurationStore) : 
            base(log, configurationStore, identityProviderConfigDiscoverer, urlBuilder, webPortalConfigurationStore)
        {
        }
    }
}