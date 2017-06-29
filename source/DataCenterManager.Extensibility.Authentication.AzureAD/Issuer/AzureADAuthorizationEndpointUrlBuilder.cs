using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IAzureADConfigurationStore>, IAzureADAuthorizationEndpointUrlBuilder
    {
        public AzureADAuthorizationEndpointUrlBuilder(IAzureADConfigurationStore configurationStore, IUrlEncoder urlEncoder) : base(configurationStore, urlEncoder)
        {
        }
    }
}