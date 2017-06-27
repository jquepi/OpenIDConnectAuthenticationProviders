using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfiguration : OpenIDConnectConfiguration
    {
        public static string DefaultRoleClaimType = "roles";

        public DataCenterManagerConfiguration() : base("DataCenterManager", "Octopus Deploy")
        {
            LoginLinkLabel = "Sign in with your Data Center Manager account";
            RoleClaimType = DefaultRoleClaimType;
        }

        public string RoleClaimType { get; set; }
    }
}