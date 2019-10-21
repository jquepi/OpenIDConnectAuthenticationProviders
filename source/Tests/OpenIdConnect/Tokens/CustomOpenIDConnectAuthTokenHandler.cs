using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Tests.OpenIdConnect.Tokens
{
    public class CustomOpenIDConnectAuthTokenHandler : OpenIDConnectAuthTokenHandler<IOpenIDConnectConfigurationStore, IKeyRetriever, IIdentityProviderConfigDiscoverer>
    { 
        public CustomOpenIDConnectAuthTokenHandler(ILog log, IOpenIDConnectConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
            
        }
    }
}
