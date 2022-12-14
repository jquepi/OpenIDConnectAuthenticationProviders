using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Certificates;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Tokens;

namespace Tests.OpenIdConnect.Tokens
{
    public class CustomOpenIDConnectAuthTokenHandler : OpenIDConnectAuthTokenHandler<IOpenIDConnectConfigurationStore, IKeyRetriever, IIdentityProviderConfigDiscoverer>
    {
        public CustomOpenIDConnectAuthTokenHandler(ISystemLog log, IOpenIDConnectConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {

        }
    }
}
