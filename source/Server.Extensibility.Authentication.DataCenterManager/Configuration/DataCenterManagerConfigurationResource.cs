using System.ComponentModel;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfigurationResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Role Claim Type")]
        [Description("The type of the role claim")]
        public string RoleClaimType { get; set; }
    }
}