using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Time;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public abstract class KeyRetriever<TStore, TKeyParser> : IKeyRetriever
        where TStore : IOpenIDConnectConfigurationStore
        where TKeyParser : IKeyJsonParser
    {
        readonly IClock clock;
        readonly TKeyParser keyParser;
        DateTime? certificateCacheExpires;
        readonly object funcLock = new object();
        Task<IDictionary<string, AsymmetricSecurityKey>> certRetrieveTask;

        protected readonly TStore ConfigurationStore;

        protected KeyRetriever(
            IClock clock,
            TStore configurationStore,
            TKeyParser keyParser)
        {
            this.clock = clock;
            ConfigurationStore = configurationStore;
            this.keyParser = keyParser;
        }

        public Task<IDictionary<string, AsymmetricSecurityKey>> GetKeysAsync(IssuerConfiguration issuerConfiguration)
        {
            lock (funcLock)
            {
                if (certRetrieveTask != null && certificateCacheExpires > clock.GetLocalTime())
                    return certRetrieveTask;

                // assume the cache is indefinite by default, and adjust back based on downloaded certificates.
                certificateCacheExpires = DateTime.MaxValue;

                certRetrieveTask = DoGetKeyAsync(issuerConfiguration);
            }
            return certRetrieveTask;
        }

        protected virtual string GetDownloadUri(IssuerConfiguration issuerConfiguration)
        {
            return issuerConfiguration.JwksUri;
        }

        public async Task<IDictionary<string, AsymmetricSecurityKey>> DoGetKeyAsync(IssuerConfiguration issuerConfiguration)
        {
            using (var client = new HttpClient())
            {
                var downloadUri = GetDownloadUri(issuerConfiguration);

                var response = await client.GetAsync((string) downloadUri);
                var content = await response.Content.ReadAsStringAsync();

                var downloadedKeys = keyParser.Parse(content);

                return downloadedKeys.ToDictionary(c => c.Kid, ConvertDetailsToKey);
            }
        }

        AsymmetricSecurityKey ConvertDetailsToKey(KeyDetails keyDetails)
        {
            var rsa = keyDetails as RsaDetails;
            if (rsa != null)
            {
                return new RsaSecurityKey(
                    new RSAParameters
                    {
                        Exponent = Base64UrlEncoder.DecodeBytes(rsa.Exponent),
                        Modulus = Base64UrlEncoder.DecodeBytes(rsa.Modulus)
                    });
            }

            var x509 = keyDetails as CertificateDetails;
            if (x509 != null)
            {
                return new X509SecurityKey(FromBase64String(x509.Kid, x509.Certificate));
            }

            throw new InvalidOperationException($"Unknown key details type: {keyDetails.GetType().Name}");
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