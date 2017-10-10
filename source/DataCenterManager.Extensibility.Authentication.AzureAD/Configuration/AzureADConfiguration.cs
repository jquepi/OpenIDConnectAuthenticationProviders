using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfiguration : OpenIDConnectConfiguration
    {
        public static string DefaultRoleClaimType = "roles";

        public AzureADConfiguration() : base("AzureAD", "Octopus Deploy")
        {
            LoginLinkLabel = "Sign in with your Azure AD account";
            RoleClaimType = DefaultRoleClaimType;
            //Id = AzureADConfigurationStore.Id;
        }

        public string RoleClaimType { get; set; }
    }
}