namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration
{
    public class OpenIDConnectConfigurationWithRole : OpenIDConnectConfiguration, IOpenIDConnectConfigurationWithRole
    {
        public OpenIDConnectConfigurationWithRole(string id) : base(id)
        {
        }

        public OpenIDConnectConfigurationWithRole(string id, string name, string author, string configurationSchemaVersion) : base(id, name, author, configurationSchemaVersion)
        {
        }

        public string RoleClaimType { get; set; }
    }
}