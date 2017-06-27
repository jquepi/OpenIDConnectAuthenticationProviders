using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Web
{
    public class DataCenterManagerHomeLinksContributor : OpenIDConnectHomeLinksContributor<IDataCenterManagerConfigurationStore, DataCenterManagerAuthenticationProvider>
    {
        public DataCenterManagerHomeLinksContributor(IDataCenterManagerConfigurationStore configurationStore, DataCenterManagerAuthenticationProvider authenticationProvider) : base(configurationStore, authenticationProvider)
        {
        }
    }
}