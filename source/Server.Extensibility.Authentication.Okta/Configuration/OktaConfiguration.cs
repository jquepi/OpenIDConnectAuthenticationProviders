using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfiguration : OpenIDConnectConfiguration
    {
        public static string DefaultRoleClaimType = "roles";

        public OktaConfiguration() : base("Okta", "Octopus Deploy")
        {
            Id = OktaConfigurationStore.SingletonId;
            LoginLinkLabel = "Sign in with your Okta account";
            RoleClaimType = DefaultRoleClaimType;
        }

        public string RoleClaimType { get; set; }
    }
}