using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.Okta.Tokens
{
    public class OktaAuthTokenHandler : OpenIDConnectAuthTokenWithRolesHandler<IOktaConfigurationStore, IKeyRetriever>, IOktaAuthTokenHandler
    {
        public OktaAuthTokenHandler(ILog log, IOktaConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }
    }
}