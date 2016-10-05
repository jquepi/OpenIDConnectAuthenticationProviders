using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer
{
    public class GoogleAppsAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IGoogleAppsConfigurationStore>, IGoogleAppsAuthorizationEndpointUrlBuilder
    {
        public GoogleAppsAuthorizationEndpointUrlBuilder(IGoogleAppsConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public override string Build(string siteBaseUri, IssuerConfiguration issuerConfiguration, string nonce, string state)
        {
            var url = base.Build(siteBaseUri, issuerConfiguration, nonce, state);

            var hd = ConfigurationStore.GetHostedDomain();
            if (!string.IsNullOrWhiteSpace(hd))
            {
                url += $"&hd={hd}";
            }

            return url;
        }
    }
}