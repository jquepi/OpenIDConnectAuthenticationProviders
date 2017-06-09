using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfiguration : OpenIDConnectConfiguration
    {
        public static string DefaultRoleClaimType = "roles";

        public AzureADConfiguration() : base("AzureAD", "Octopus Deploy")
        {
            LoginLinkLabel = "Sign in with your Azure AD account";
            RoleClaimType = DefaultRoleClaimType;
        }

        public string RoleClaimType { get; set; }
    }
}