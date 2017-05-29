using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;

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