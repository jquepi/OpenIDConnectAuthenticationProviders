using Octopus.Data.Model;
using Octopus.Data.Resources;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationResource : IResource
    {
        public string RoleClaimType { get; set; }

        public string Id { get; }

        public LinkCollection Links { get; set; }
    }
}