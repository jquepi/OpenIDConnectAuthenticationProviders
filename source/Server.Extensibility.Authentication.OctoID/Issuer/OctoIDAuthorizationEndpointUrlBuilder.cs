using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Issuer
{
    public class OctoIDAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IOctoIDConfigurationStore>, IOctoIDAuthorizationEndpointUrlBuilder
    {
        public OctoIDAuthorizationEndpointUrlBuilder(IOctoIDConfigurationStore configurationStore, IUrlEncoder urlEncoder) : base(configurationStore, urlEncoder)
        {
        }
    }
}