using System.ComponentModel;
using Octopus.Data.Resources.Attributes;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfigurationResource : OpenIDConnectConfigurationResource
    {
        [DisplayName("Hosted Domain")]
        [Writeable]
        public string HostedDomain { get; set; }
    }
}