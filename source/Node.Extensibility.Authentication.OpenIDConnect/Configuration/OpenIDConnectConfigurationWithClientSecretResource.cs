using System.ComponentModel;
using Octopus.Data.Resources;
using Octopus.Data.Resources.Attributes;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationWithClientSecretResource : OpenIDConnectConfigurationResource
    {
        
        [DisplayName("Client Secret")]
        [Description("Shared secret for validating the authentication tokens")]
        [Writeable]
        public SensitiveValue ClientSecret { get; set; }
    }
}