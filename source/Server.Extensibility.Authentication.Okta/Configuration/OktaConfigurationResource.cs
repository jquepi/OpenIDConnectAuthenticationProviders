using Octopus.Data.Model;
using Octopus.Data.Resources;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfigurationResource : IResource
    {
        public string Id { get; }

        public LinkCollection Links { get; set; }
    }
}