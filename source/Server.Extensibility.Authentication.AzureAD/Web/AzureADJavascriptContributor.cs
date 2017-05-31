using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Web;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADJavascriptContributor : OpenIDConnectJavascriptContributor<IAzureADConfigurationStore>
    {
        public AzureADJavascriptContributor(IAzureADConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string AngularModuleNameSuffix => "azureAD";
        public override string JavascriptFilenamePrefix => "azureAD";
    }
}