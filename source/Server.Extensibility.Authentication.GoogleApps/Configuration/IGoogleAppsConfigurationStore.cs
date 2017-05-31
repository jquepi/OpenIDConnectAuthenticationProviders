using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public interface IGoogleAppsConfigurationStore : IOpenIDConnectConfigurationStore
    {
        string GetCertificateUri();
        void SetCertificateUri(string certificateUri);

        string GetHostedDomain();
        void SetHostedDomain(string hostedDomain);
    }
}