using System.ComponentModel;
using Octopus.Data.Resources.Attributes;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    [Description("Sign in to your Octopus Server with Okta. [Learn more](https://g.octopushq.com/AuthOkta).")]
    public class OktaConfigurationResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Role Claim Type")]
        [Description("Tell Octopus how to find the roles in the security token from Okta")]
        [Writeable]
        public string RoleClaimType { get; set; }
    }
}