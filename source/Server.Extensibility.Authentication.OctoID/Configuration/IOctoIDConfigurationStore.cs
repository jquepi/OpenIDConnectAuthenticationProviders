using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public interface IOctoIDConfigurationStore : IOpenIDConnectConfigurationStore<OctoIDConfiguration>
    {
        string GetUsernameClaimType();
        void SetUsernameClaimType(string usernameClaimType);
    }
}