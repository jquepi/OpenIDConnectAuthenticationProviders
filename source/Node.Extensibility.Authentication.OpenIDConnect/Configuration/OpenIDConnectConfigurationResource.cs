using System.ComponentModel;
using Octopus.Data.Resources.Attributes;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationResource : ExtensionConfigurationResource
    {
        [Description("Follow our documentation to find the Issuer for your identity provider")]
        [Writeable]
        public virtual string Issuer { get; set; }

        [DisplayName("Client ID")]
        [Description("Octopus instances unique authentication id, as provided by ????")]
        [Writeable]
        public string ClientId { get; set; }

        [DisplayName("Client Secret")]
        [Description("Shared secret for validating the authentication tokens")]
        [Writeable]
        public string ClientSecret { get; set; }

        [DisplayName("Allow Auto User Creation")]
        [Description("Tell Octopus to automatically create a user account when a person signs in for the first time with this identity provider")]
        [Writeable]
        public bool? AllowAutoUserCreation { get; set; }
    }
}