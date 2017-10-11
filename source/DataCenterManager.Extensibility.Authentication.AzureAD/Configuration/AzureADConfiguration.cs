using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfiguration : OpenIDConnectConfiguration
    {
        public static string DefaultRoleClaimType = "roles";

        public AzureADConfiguration() : base("AzureAD", "Octopus Deploy", "1.0")
        {
            Id = AzureADConfigurationStore.SingletonId;
            LoginLinkLabel = "Sign in with your Azure AD account";
            RoleClaimType = DefaultRoleClaimType;
        }

        public string RoleClaimType { get; set; }
    }
}