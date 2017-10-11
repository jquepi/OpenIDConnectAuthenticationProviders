using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfiguration : OpenIDConnectConfiguration
    {
        public GoogleAppsConfiguration() : base("GoogleApps", "Octopus Deploy", "1.0")
        {
            Id = GoogleAppsConfigurationStore.SingletonId;
            Issuer = "https://accounts.google.com";
            LoginLinkLabel = "Sign in with your Google Apps account";
        }

        public string HostedDomain { get; set; }
    }
}