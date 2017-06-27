using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public interface IDataCenterManagerConfigurationStore : IOpenIDConnectConfigurationStore
    {
        string GetRoleClaimType();
        void SetRoleClaimType(string roleClaimType);
    }
}