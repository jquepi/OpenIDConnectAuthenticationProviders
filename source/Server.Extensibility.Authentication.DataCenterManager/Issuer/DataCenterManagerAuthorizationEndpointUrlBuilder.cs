using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Issuer
{
    public class DataCenterManagerAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IDataCenterManagerConfigurationStore>, IDataCenterManagerAuthorizationEndpointUrlBuilder
    {
        public DataCenterManagerAuthorizationEndpointUrlBuilder(IDataCenterManagerConfigurationStore configurationStore, IUrlEncoder urlEncoder) : base(configurationStore, urlEncoder)
        {
        }
    }
}