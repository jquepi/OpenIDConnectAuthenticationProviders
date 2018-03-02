using Nevermore.Contracts;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public interface IOpenIDConnectConfigurationWithRoleStore<TConfiguration> : IOpenIDConnectConfigurationStore<TConfiguration>, IOpenIDConnectConfigurationWithRoleStore
        where TConfiguration : OpenIDConnectConfigurationWithRole, IId, new()
    {
    }

    public interface IOpenIDConnectConfigurationWithRoleStore : IOpenIDConnectConfigurationStore
    {
        string GetRoleClaimType();
        void SetRoleClaimType(string roleClaimType);
    }
}