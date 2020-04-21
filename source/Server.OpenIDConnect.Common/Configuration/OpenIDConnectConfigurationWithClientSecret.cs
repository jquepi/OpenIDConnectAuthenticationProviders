using Octopus.Data.Model;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration
{
    public class OpenIDConnectConfigurationWithClientSecret : OpenIDConnectConfiguration, IOpenIDConnectConfigurationWithClientSecret
    {
        public OpenIDConnectConfigurationWithClientSecret(string id) : base(id)
        {
        }

        public OpenIDConnectConfigurationWithClientSecret(string id, string name, string author, string configurationSchemaVersion) : base(id, name, author, configurationSchemaVersion)
        {
        }

        public SensitiveString ClientSecret { get; set; }
    }
}