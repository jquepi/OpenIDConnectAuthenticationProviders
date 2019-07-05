using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctoID.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Web
{
    public class OctoIDUserAuthenticationAction : UserAuthenticationAction<IOctoIDConfigurationStore>
    {
        public OctoIDUserAuthenticationAction(
            ILog log,
            IOctoIDConfigurationStore configurationStore, 
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IOctoIDAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionModelBinder modelBinder,
            IAuthenticationConfigurationStore authenticationConfigurationStore) : base(log, configurationStore, identityProviderConfigDiscoverer, urlBuilder, modelBinder, authenticationConfigurationStore)
        {
        }
    }
}