using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctopusID.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Tokens
{
    public class OctopusIDAuthTokenHandler : OpenIDConnectAuthTokenWithRolesHandler<IOctopusIDConfigurationStore, IOctopusIDKeyRetriever>, IOctopusIDAuthTokenHandler
    {
        public OctopusIDAuthTokenHandler(ILog log, IOctopusIDConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IOctopusIDKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }
    }
}