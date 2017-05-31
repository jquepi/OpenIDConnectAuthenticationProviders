using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer
{
    public class GoogleCertificateRetriever : CertificateRetriever<IGoogleAppsConfigurationStore, IGoogleCertificateJsonParser>, IGoogleCertificateRetriever
    {
        public GoogleCertificateRetriever(IClock clock, IGoogleAppsConfigurationStore configurationStore, IGoogleCertificateJsonParser certificateParser) : base(clock, configurationStore, certificateParser)
        {
        }

        protected override string GetDownloadUri(IssuerConfiguration issuerConfiguration)
        {
            var downloadUri = ConfigurationStore.GetCertificateUri();
            if (string.IsNullOrWhiteSpace(downloadUri))
            {
                downloadUri = issuerConfiguration.JwksUri;
            }

            return downloadUri;
        }
    }
}