using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Issuer;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Tokens
{
    public class AzureADAuthTokenHandler : OpenIDConnectAuthTokenHandler<IAzureADConfigurationStore, AzureADKeyRetriever>, IAzureADAuthTokenHandler
    {
        public AzureADAuthTokenHandler(
            ILog log, 
            IAzureADConfigurationStore configurationStore, 
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            AzureADKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }
    }
}