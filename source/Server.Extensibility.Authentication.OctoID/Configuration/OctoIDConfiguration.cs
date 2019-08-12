using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfiguration : OpenIDConnectConfigurationWithClientSecret, IOpenIDConnectConfigurationWithRole
    {
        public static string DefaultRoleClaimType = "roles";

        public OctoIDConfiguration() : base("OctopusID", "Octopus Deploy", "1.0")
        {
            Id = OctoIDConfigurationStore.SingletonId;
            Issuer = "https://account.octopus.com";
            Scope = DefaultScope;
            RoleClaimType = DefaultRoleClaimType;
            IsEnabled = true;
        }
        
        public string RoleClaimType { get; set; }
    }
}