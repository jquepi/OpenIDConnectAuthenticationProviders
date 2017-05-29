using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public abstract class CertificateRetriever<TStore, TCertificateParser> : ICertificateRetriever
        where TStore : IOpenIDConnectConfigurationStore
        where TCertificateParser : ICertificateJsonParser
    {
        readonly IClock clock;
        readonly TCertificateParser certificateParser;
        DateTime? certificateCacheExpires;
        readonly object funcLock = new object();
        Task<IDictionary<string, X509Certificate2>> certRetrieveTask;

        protected readonly TStore ConfigurationStore;

        protected CertificateRetriever(
            IClock clock,
            TStore configurationStore,
            TCertificateParser certificateParser)
        {
            this.clock = clock;
            ConfigurationStore = configurationStore;
            this.certificateParser = certificateParser;
        }
 
        public Task<IDictionary<string, X509Certificate2>> GetCertificatesAsync(IssuerConfiguration issuerConfiguration)
        {
            lock (funcLock)
            {
                if (certRetrieveTask != null && certificateCacheExpires > clock.GetLocalTime())
                    return certRetrieveTask;

                // assume the cache is indefinite by default, and adjust back based on downloaded certificates.
                certificateCacheExpires = DateTime.MaxValue;

                certRetrieveTask = DoGetCertificateAsync(issuerConfiguration);
            }
            return certRetrieveTask;
        }

        protected virtual string GetDownloadUri(IssuerConfiguration issuerConfiguration)
        {
            return issuerConfiguration.JwksUri;
        }

        public async Task<IDictionary<string, X509Certificate2>> DoGetCertificateAsync(IssuerConfiguration issuerConfiguration)
        {
            using (var client = new HttpClient())
            {
                var downloadUri = GetDownloadUri(issuerConfiguration);

                var response = await client.GetAsync(downloadUri);
                var content = await response.Content.ReadAsStringAsync();

                var downloadedCerts = certificateParser.Parse(content);

                return downloadedCerts.ToDictionary(c => c.Kid, c => FromBase64String(c.Kid, c.Certificate));
            }
        }

        X509Certificate2 FromBase64String(string kid, string certificateString)
        {
            // Load the certificate via a file, per http://paulstovell.com/blog/x509certificate2
            var raw = Convert.FromBase64String(certificateString);
            var file = Path.Combine(Path.GetTempPath(), kid);

            try
            {
                File.WriteAllBytes(file, raw);

                var certificate = new X509Certificate2(file);

                if (certificate.NotAfter < certificateCacheExpires)
                {
                    // eagerly refresh the cache in the last 5min before the certificate will expire.
                    certificateCacheExpires = certificate.NotAfter.Subtract(TimeSpan.FromMinutes(5));
                }

                return certificate;
            }
            finally
            {
                File.Delete(file);
            }
        }
    }
}