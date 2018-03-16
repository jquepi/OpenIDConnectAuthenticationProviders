using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfiguration : OpenIDConnectConfigurationWithRole
    {
        public static string DefaultRoleClaimType = "roles";

        public AzureADConfiguration() : base("AzureAD", "Octopus Deploy", "1.0")
        {
            Id = AzureADConfigurationStore.SingletonId;
            RoleClaimType = DefaultRoleClaimType;
        }
    }
}