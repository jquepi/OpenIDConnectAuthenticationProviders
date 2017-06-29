using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADHomeLinksContributor : OpenIDConnectHomeLinksContributor<IAzureADConfigurationStore, AzureADAuthenticationProvider>
    {
        public AzureADHomeLinksContributor(IAzureADConfigurationStore configurationStore, AzureADAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}