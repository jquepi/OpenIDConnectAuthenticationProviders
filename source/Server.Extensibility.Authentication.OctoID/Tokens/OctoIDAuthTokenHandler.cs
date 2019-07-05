using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctoID.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Tokens
{
    public class OctoIDAuthTokenHandler : OpenIDConnectAuthTokenHandler<IOctoIDConfigurationStore, IOctoIDKeyRetriever>, IOctoIDAuthTokenHandler
    {
        public OctoIDAuthTokenHandler(ILog log, IOctoIDConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IOctoIDKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }
    }
}