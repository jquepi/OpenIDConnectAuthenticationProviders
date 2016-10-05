namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public interface ICertificateJsonParser
    {
        CertificateDetails[] Parse(string content);
    }
}