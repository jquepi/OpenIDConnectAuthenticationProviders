using Octopus.Data.Model;

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

        [Encrypted]
        public string ClientSecret { get; set; }
    }
}