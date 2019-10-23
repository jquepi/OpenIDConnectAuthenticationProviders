using Nevermore.Contracts;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration
{
    public interface IOpenIDConnectWithClientSecretConfigurationStore<TConfiguration> : IOpenIDConnectConfigurationStore<TConfiguration>, IOpenIDConnectWithClientSecretConfigurationStore
        where TConfiguration : OpenIDConnectConfiguration, IId, new()
    {
    }

    public interface IOpenIDConnectWithClientSecretConfigurationStore : IOpenIDConnectConfigurationStore
    {
        string GetClientSecret();
        void SetClientSecret(string clientSecret);
    }
}