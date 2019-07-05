using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Web
{
    public class OctoIDHomeLinksContributor : OpenIDConnectHomeLinksContributor<IOctoIDConfigurationStore, OctoIDAuthenticationProvider>
    {
        public OctoIDHomeLinksContributor(IOctoIDConfigurationStore configurationStore, OctoIDAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}