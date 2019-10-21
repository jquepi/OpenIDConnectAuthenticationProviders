using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Web
{
    public class OctopusIDHomeLinksContributor : OpenIDConnectHomeLinksContributor<IOctopusIDConfigurationStore, OctopusIDAuthenticationProvider>
    {
        public OctopusIDHomeLinksContributor(IOctopusIDConfigurationStore configurationStore, OctopusIDAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}