using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Tests.OpenIdConnect.Tokens
{
    public class CustomOpenIDConnectWithRoleAuthTokenHandler : OpenIDConnectAuthTokenWithRolesHandler<IOpenIDConnectConfigurationWithRoleStore, IKeyRetriever, IIdentityProviderConfigDiscoverer>
    { 
        public CustomOpenIDConnectWithRoleAuthTokenHandler(ILog log, IOpenIDConnectConfigurationWithRoleStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
            
        }
    }
}