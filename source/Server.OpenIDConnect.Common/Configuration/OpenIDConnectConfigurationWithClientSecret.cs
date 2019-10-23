using Octopus.Data.Model;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration
{
    public class OpenIDConnectConfigurationWithClientSecret : OpenIDConnectConfiguration, IOpenIDConnectConfigurationWithClientSecret
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