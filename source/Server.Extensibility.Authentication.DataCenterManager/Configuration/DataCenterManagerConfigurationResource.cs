using Octopus.Data.Model;
using Octopus.Data.Resources;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfigurationResource : IResource
    {
        public string Id { get; }

        public LinkCollection Links { get; set; }
    }
}