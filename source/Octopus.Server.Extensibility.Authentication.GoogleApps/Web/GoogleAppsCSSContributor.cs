using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsCSSContributor : OpenIDConnectCSSContributor<IGoogleAppsConfigurationStore>
    {
        public GoogleAppsCSSContributor(IGoogleAppsConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public override string CSSFilename => "googleApps";
    }
}