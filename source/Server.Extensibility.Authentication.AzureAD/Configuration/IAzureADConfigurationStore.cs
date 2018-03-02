using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public interface IAzureADConfigurationStore : IOpenIDConnectConfigurationWithRoleStore<AzureADConfiguration>
    {
    }
}