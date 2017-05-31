using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsJavascriptContributor : OpenIDConnectJavascriptContributor<IGoogleAppsConfigurationStore>
    {
        public GoogleAppsJavascriptContributor(IGoogleAppsConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string AngularModuleNameSuffix => "google";
        public override string JavascriptFilenamePrefix => "googleApps";
    }
}