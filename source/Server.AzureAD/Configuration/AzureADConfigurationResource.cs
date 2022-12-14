using System.ComponentModel;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;
using Octopus.Server.MessageContracts.Attributes;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    [Description("Sign in to your Octopus Server with Azure Active Directory. [Learn more](https://g.octopushq.com/AuthAzureAD).")]
    class AzureADConfigurationResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Role Claim Type")]
        [Description("Tell Octopus how to find the roles/groups in the security token from Azure Active Directory (usually \"roles\" or \"groups\")")]
        [Writeable]
        public string? RoleClaimType { get; set; }
    }
}