using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADCertificateRetriever : CertificateRetriever<IAzureADConfigurationStore, ICertificateJsonParser>, IAzureADCertificateRetriever
    {
        public AzureADCertificateRetriever(IClock clock, IAzureADConfigurationStore configurationStore, ICertificateJsonParser certificateParser) : base(clock, configurationStore, certificateParser)
        {
        }
    }
}