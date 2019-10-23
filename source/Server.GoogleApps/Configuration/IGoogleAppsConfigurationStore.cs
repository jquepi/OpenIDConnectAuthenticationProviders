using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public interface IGoogleAppsConfigurationStore : IOpenIDConnectConfigurationStore<GoogleAppsConfiguration>
    {
        string GetHostedDomain();
        void SetHostedDomain(string hostedDomain);
    }
}