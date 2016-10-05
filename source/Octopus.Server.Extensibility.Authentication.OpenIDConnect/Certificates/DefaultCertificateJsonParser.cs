using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public class DefaultCertificateJsonParser : ICertificateJsonParser
    {
        public CertificateDetails[] Parse(string content)
        {
            var keyData = JsonConvert.DeserializeObject<IssuerKeys>(content);

            return keyData.Keys
                .Select(cert => new CertificateDetails
                {
                    Kid = cert.kid,
                    Certificate = cert.x5c.First()
                })
                .ToArray();

        }
        public class IssuerKeys
        {
            public List<IssuerKey> Keys { get; set; }
        }

        public class IssuerKey
        {
            public string kid { get; set; }
            public string[] x5c { get; set; }
        }
    }
}