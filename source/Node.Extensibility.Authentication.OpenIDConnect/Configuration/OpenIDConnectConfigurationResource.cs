using System.ComponentModel;
using Octopus.Data.Resources.Attributes;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIDConnectConfigurationResource : ExtensionConfigurationResource
    {
        [Description("Set the issuer, used for authentication")]
        [Writeable]
        public string Issuer { get; set; }

        [DisplayName("Response Type")]
        [Writeable]
        public string ResponseType { get; set; }

        [DisplayName("Response Mode")]
        [Writeable]
        public string ResponseMode { get; set; }

        [DisplayName("Client ID")]
        [Writeable]
        public string ClientId { get; set; }

        [Writeable]
        public string Scope { get; set; }

        [DisplayName("Name Claim Type")]
        [Writeable]
        public string NameClaimType { get; set; }

        [DisplayName("Login Link Label")]
        [Writeable]
        public string LoginLinkLabel { get; set; }

        [DisplayName("Allow Auto User Creation")]
        [Writeable]
        public bool? AllowAutoUserCreation { get; set; }
    }
}