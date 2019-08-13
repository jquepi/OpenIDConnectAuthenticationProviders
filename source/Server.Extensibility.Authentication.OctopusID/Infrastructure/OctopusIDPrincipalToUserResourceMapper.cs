using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Infrastructure
{
    public class OctopusIDPrincipalToUserResourceMapper : PrincipalToUserResourceMapper, IOctopusIDPrincipalToUserResourceMapper
    {
        readonly IOctopusIDConfigurationStore configurationStore;

        public OctopusIDPrincipalToUserResourceMapper(IOctopusIDConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }
    }
}