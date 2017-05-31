using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens
{
    public class GoogleCertificateJsonParser : IGoogleCertificateJsonParser
    {
        public CertificateDetails[] Parse(string content)
        {
            var keyData = JsonConvert.DeserializeObject<Dictionary<string,string>>(content);

            return keyData
                .Select(cert => new CertificateDetails
                {
                    Kid = cert.Key,
                    Certificate = cert.Value.Replace("-----BEGIN CERTIFICATE-----", string.Empty).Replace(@"-----END CERTIFICATE-----", string.Empty).Replace("\n", string.Empty)
                })
                .ToArray();
        }
    }
}