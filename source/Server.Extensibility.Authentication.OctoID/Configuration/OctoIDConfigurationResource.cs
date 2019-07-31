using System.ComponentModel;
using Octopus.Data.Resources.Attributes;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    [Description("Sign in to your Octopus Server with an Octopus ID. [Learn more](https://g.octopushq.com/AuthOctoID).")]
    public class OctoIDConfigurationResource : OpenIDConnectConfigurationWithClientSecretResource
    {
        [ReadOnly(true)]
        public override string Issuer { get; set; }
    }
}