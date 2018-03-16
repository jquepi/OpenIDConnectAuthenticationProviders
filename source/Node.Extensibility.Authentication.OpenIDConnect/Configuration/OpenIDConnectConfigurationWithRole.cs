namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationWithRole : OpenIDConnectConfiguration
    {
        public OpenIDConnectConfigurationWithRole()
        {
        }

        public OpenIDConnectConfigurationWithRole(string name, string author, string configurationSchemaVersion) : base(name, author, configurationSchemaVersion)
        {
        }

        public string RoleClaimType { get; set; }
    }
}