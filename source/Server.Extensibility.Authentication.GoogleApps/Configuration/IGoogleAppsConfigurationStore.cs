using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public interface IGoogleAppsConfigurationStore : IOpenIDConnectConfigurationStore<GoogleAppsConfiguration>
    {
        string GetHostedDomain();
        void SetHostedDomain(string hostedDomain);
    }
}