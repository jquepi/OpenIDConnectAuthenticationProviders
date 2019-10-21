using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Issuer
{
    public class OctopusIDAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IOctopusIDConfigurationStore>, IOctopusIDAuthorizationEndpointUrlBuilder
    {
        public OctopusIDAuthorizationEndpointUrlBuilder(IOctopusIDConfigurationStore configurationStore, IUrlEncoder urlEncoder) : base(configurationStore, urlEncoder)
        {
        }
    }
}