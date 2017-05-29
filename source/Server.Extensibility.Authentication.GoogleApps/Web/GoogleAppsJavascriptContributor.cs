using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;

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