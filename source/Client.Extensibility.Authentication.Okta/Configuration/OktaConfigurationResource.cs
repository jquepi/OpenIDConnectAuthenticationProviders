using System.ComponentModel;
using Octopus.Client.Extensibility.Attributes;
using Octopus.Client.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Client.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfigurationResource : OpenIDConnectConfigurationResource
    {
        public OktaConfigurationResource()
        {
            Id = "authentication-od";
        }

        [DisplayName("Role Claim Type")]
        [Description("The type of the role claim")]
        [Writeable]
        public string RoleClaimType { get; set; }
    }
}