using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfiguration : OpenIDConnectConfiguration
    {
        public GoogleAppsConfiguration() : base("GoogleApps", "Octopus Deploy", "1.0")
        {
            Id = GoogleAppsConfigurationStore.SingletonId;
            Issuer = "https://accounts.google.com";
        }

        public string HostedDomain { get; set; }
    }
}