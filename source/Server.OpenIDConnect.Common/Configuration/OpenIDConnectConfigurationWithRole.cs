namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationWithRole : OpenIDConnectConfiguration, IOpenIDConnectConfigurationWithRole
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