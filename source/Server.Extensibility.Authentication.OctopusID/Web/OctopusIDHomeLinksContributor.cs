using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Web
{
    public class OctopusIDHomeLinksContributor : OpenIDConnectHomeLinksContributor<IOctopusIDConfigurationStore, OctopusIDAuthenticationProvider>
    {
        public OctopusIDHomeLinksContributor(IOctopusIDConfigurationStore configurationStore, OctopusIDAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}