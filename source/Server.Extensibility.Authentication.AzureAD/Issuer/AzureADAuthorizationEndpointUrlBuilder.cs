using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IAzureADConfigurationStore>, IAzureADAuthorizationEndpointUrlBuilder
    {
        public AzureADAuthorizationEndpointUrlBuilder(IAzureADConfigurationStore configurationStore, IUrlEncoder urlEncoder) : base(configurationStore, urlEncoder)
        {
        }
    }
}