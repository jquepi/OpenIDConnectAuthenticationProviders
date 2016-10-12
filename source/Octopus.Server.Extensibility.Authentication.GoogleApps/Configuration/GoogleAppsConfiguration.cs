using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfiguration : OpenIDConnectConfiguration
    {
        public const string DefaultCertificateUri = "https://www.googleapis.com/oauth2/v1/certs";

        public GoogleAppsConfiguration() : base("GoogleApps", "Octopus Deploy")
        {
            Issuer = "https://accounts.google.com";
            CertificateUri = DefaultCertificateUri;
            LoginLinkLabel = "Sign in with your Google Apps account";
        }

        public string CertificateUri { get; set; }

        public string HostedDomain { get; set; }
    }
}