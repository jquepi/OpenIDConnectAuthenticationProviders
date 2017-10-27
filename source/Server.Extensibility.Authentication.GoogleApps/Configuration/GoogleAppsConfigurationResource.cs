using Octopus.Data.Model;
using Octopus.Data.Resources;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfigurationResource : IResource
    {
        public string Id { get; set; }

        public LinkCollection Links { get; set; }
    }
}