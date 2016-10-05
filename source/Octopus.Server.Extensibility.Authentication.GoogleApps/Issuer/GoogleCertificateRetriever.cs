using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.HostServices.Time;

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