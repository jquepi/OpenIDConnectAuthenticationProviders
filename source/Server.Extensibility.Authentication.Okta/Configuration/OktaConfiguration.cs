using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfiguration : OpenIDConnectConfigurationWithRole
    {
        public static string DefaultRoleClaimType = "groups";

        public OktaConfiguration() : base("Okta", "Octopus Deploy", "1.0")
        {
            Id = OktaConfigurationStore.SingletonId;
            RoleClaimType = DefaultRoleClaimType;
            Scope = DefaultScope + "%20groups";
        }
    }
}