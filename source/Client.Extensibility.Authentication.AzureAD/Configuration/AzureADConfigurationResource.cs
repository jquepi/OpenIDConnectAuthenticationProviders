using System.ComponentModel;
using Octopus.Client.Extensibility.Attributes;
using Octopus.Client.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Client.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationResource : OpenIDConnectConfigurationResource
    {
        public AzureADConfigurationResource()
        {
            Id = "authentication-aad";
        }

        [DisplayName("Role Claim Type")]
        [Description("Tell Octopus how to find the roles in the security token from Azure Active Directory")]
        [Writeable]
        public string RoleClaimType { get; set; }
    }
}