using Octopus.Data.Model;
using Octopus.Data.Resources;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationResource : IResource
    {
        public string Id { get; }

        public LinkCollection Links { get; set; }
    }
}