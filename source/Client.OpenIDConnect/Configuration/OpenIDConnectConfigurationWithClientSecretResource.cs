using System.ComponentModel;
using Octopus.Client.Extensibility.Attributes;
using Octopus.Client.Model;

namespace Octopus.Client.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationWithClientSecretResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Client Secret")]
        [Description("Follow our documentation to find the Client Secret for your identity provider")]
        [Writeable]
        public SensitiveValue ClientSecret { get; set; }

    }
}