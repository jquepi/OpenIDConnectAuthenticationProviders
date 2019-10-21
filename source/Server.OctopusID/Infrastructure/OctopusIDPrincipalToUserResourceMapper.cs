using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;

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