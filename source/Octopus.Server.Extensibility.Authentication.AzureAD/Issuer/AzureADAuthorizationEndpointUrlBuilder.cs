using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IAzureADConfigurationStore>, IAzureADAuthorizationEndpointUrlBuilder
    {
        public AzureADAuthorizationEndpointUrlBuilder(IAzureADConfigurationStore configurationStore) : base(configurationStore)
        {
        }
    }
}