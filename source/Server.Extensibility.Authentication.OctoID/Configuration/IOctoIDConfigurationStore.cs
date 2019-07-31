using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public interface IOctoIDConfigurationStore : IOpenIDConnectWithClientSecretConfigurationStore<OctoIDConfiguration>, 
        IOpenIDConnectConfigurationWithRoleStore<OctoIDConfiguration>
    {
    }
}