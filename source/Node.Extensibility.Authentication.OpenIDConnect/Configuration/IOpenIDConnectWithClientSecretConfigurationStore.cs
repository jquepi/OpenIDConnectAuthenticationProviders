using Nevermore.Contracts;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
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