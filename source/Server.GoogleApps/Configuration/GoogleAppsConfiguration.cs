using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    class GoogleAppsConfiguration : OpenIDConnectConfiguration
    {
        public GoogleAppsConfiguration() : base("GoogleApps", "Octopus Deploy", "1.0")
        {
            Id = GoogleAppsConfigurationStore.SingletonId;
            Issuer = "https://accounts.google.com";
        }

        public string HostedDomain { get; set; }
    }
}