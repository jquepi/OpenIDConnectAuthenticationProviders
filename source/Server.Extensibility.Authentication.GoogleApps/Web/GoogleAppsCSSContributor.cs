using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;

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