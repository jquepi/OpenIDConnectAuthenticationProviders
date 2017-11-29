using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public interface IOktaConfigurationStore : IOpenIDConnectConfigurationStore<OktaConfiguration>
    {
        string GetRoleClaimType();
        void SetRoleClaimType(string roleClaimType);
    }
}