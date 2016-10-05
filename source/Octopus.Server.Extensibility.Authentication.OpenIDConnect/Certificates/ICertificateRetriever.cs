using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public interface ICertificateRetriever
    {
        Task<IDictionary<string, X509Certificate2>> GetCertificatesAsync(IssuerConfiguration issuerConfiguration);
    }
}