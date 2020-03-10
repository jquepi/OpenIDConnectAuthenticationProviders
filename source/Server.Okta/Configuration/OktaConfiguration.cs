using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    class OktaConfiguration : OpenIDConnectConfigurationWithRole
    {
        public static string DefaultRoleClaimType = "groups";
        public static string DefaultUsernameClaimType = "preferred_username";

        public OktaConfiguration() : base("Okta", "Octopus Deploy", "1.0")
        {
            Id = OktaConfigurationStore.SingletonId;
            RoleClaimType = DefaultRoleClaimType;
            UsernameClaimType = DefaultUsernameClaimType;
            Scope = DefaultScope + "%20groups";
        }

        public string UsernameClaimType { get; set; }
    }
}