using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADCSSContributor : OpenIDConnectCSSContributor<IAzureADConfigurationStore>
    {
        public AzureADCSSContributor(IAzureADConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public override string CSSFilename => "azureAD";
    }
}