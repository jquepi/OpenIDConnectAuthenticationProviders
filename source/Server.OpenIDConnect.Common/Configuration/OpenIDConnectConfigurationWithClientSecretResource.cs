using System.ComponentModel;
using Octopus.Server.MessageContracts;
using Octopus.Server.MessageContracts.Attributes;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration
{
    public class OpenIDConnectConfigurationWithClientSecretResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Client Secret")]
        [Description("Shared secret for validating the authentication tokens")]
        [Writeable]
        public virtual SensitiveValue? ClientSecret { get; set; }
    }
}