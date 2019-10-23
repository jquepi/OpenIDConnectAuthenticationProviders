using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public interface IAzureADConfigurationStore : IOpenIDConnectConfigurationWithRoleStore<AzureADConfiguration>
    {
    }
}