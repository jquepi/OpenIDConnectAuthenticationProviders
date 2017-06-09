using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADHomeLinksContributor : OpenIDConnectHomeLinksContributor<IAzureADConfigurationStore, AzureADAuthenticationProvider>
    {
        public AzureADHomeLinksContributor(IAzureADConfigurationStore configurationStore, AzureADAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}