using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Web
{
    public class DataCenterManagerUserAuthenticationAction : UserAuthenticationAction<IDataCenterManagerConfigurationStore>
    {
        public DataCenterManagerUserAuthenticationAction(
            ILog log,
            IDataCenterManagerConfigurationStore configurationStore, 
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IDataCenterManagerAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionResponseCreator responseCreator,
            IApiActionModelBinder modelBinder,
            IWebPortalConfigurationStore webPortalConfigurationStore) : base(log, configurationStore, identityProviderConfigDiscoverer, urlBuilder, responseCreator, modelBinder, webPortalConfigurationStore)
        {
        }
    }
}