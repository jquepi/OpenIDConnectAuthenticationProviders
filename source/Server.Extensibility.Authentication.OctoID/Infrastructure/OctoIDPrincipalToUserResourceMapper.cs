using System.Security.Claims;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Infrastructure
{
    public class OctoIDPrincipalToUserResourceMapper : PrincipalToUserResourceMapper, IOctoIDPrincipalToUserResourceMapper
    {
        readonly IOctoIDConfigurationStore configurationStore;

        public OctoIDPrincipalToUserResourceMapper(IOctoIDConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }
    }
}