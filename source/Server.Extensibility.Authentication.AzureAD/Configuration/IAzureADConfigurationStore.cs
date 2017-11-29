using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public interface IAzureADConfigurationStore : IOpenIDConnectConfigurationStore<AzureADConfiguration>
    {
        string GetRoleClaimType();
        void SetRoleClaimType(string roleClaimType);
    }
}