using System.ComponentModel;
using Octopus.Client.Extensibility.Attributes;
using Octopus.Client.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Client.Extensibility.Authentication.OctoID.Configuration
{
    [Description("Sign in to your Octopus Server with your Octopus ID. [Learn more](https://g.octopushq.com/AuthOctoID).")]
    public class OctoIDConfigurationResource : OpenIDConnectConfigurationWithClientSecretResource
    {
        public OctoIDConfigurationResource()
        {
            Id = "authentication-octoid";
        }
    }
}