using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Configuration
{
    public interface IOctopusIDConfigurationStore : IOpenIDConnectWithClientSecretConfigurationStore<OctopusIDConfiguration>, 
        IOpenIDConnectConfigurationWithRoleStore<OctopusIDConfiguration>
    {
    }
}