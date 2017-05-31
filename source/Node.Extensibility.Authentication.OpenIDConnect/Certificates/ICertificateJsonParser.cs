namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates
{
    public interface ICertificateJsonParser
    {
        CertificateDetails[] Parse(string content);
    }
}