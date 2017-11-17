using System.ComponentModel;
using Octopus.Client.Extensibility.Attributes;
using Octopus.Client.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Client.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationResource : ExtensionConfigurationResource
    {
        [Description("Set the issuer, used for authentication")]
        [Writeable]
        public string Issuer { get; set; }

        [DisplayName("Client ID")]
        [Writeable]
        public string ClientId { get; set; }

        [Writeable]
        public string Scope { get; set; }

        [DisplayName("Name Claim Type")]
        [Writeable]
        public string NameClaimType { get; set; }

        [DisplayName("Allow Auto User Creation")]
        [Writeable]
        public bool? AllowAutoUserCreation { get; set; }
    }
}