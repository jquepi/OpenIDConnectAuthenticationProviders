namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationWithClientSecret : OpenIDConnectConfiguration
    {
        public OpenIDConnectConfigurationWithClientSecret()
        {
        }

        public OpenIDConnectConfigurationWithClientSecret(string name, string author, string configurationSchemaVersion) : base(name, author, configurationSchemaVersion)
        {
        }

        public string ClientSecret { get; set; }
    }
}