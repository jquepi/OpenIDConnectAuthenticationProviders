using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public interface IAzureADConfigurationStore : IOpenIDConnectConfigurationStore
    {
        string GetRoleClaimType();
        void SetRoleClaimType(string roleClaimType);
    }
}