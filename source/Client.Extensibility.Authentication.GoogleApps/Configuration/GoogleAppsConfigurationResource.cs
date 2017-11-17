using System.ComponentModel;
using Octopus.Client.Extensibility.Attributes;
using Octopus.Client.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Client.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfigurationResource : OpenIDConnectConfigurationResource
    {
        public GoogleAppsConfigurationResource()
        {
            Id = "authentication-googleapps";
        }

        [DisplayName("Hosted Domain")]
        [Writeable]
        public string HostedDomain { get; set; }
    }
}