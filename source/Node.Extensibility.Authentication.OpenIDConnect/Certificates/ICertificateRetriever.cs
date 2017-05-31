using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates
{
    public interface ICertificateRetriever
    {
        Task<IDictionary<string, X509Certificate2>> GetCertificatesAsync(IssuerConfiguration issuerConfiguration);
    }
}