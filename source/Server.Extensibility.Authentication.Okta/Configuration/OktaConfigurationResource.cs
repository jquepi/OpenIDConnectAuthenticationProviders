using System.ComponentModel;
using Octopus.Data.Resources.Attributes;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfigurationResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Role Claim Type")]
        [Description("The type of the role claim")]
        [Writeable]
        public string RoleClaimType { get; set; }
    }
}