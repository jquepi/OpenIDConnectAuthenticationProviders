using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Web
{
    public class OktaHomeLinksContributor : OpenIDConnectHomeLinksContributor<IOktaConfigurationStore, OktaAuthenticationProvider>
    {
        public OktaHomeLinksContributor(IOktaConfigurationStore configurationStore, OktaAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}