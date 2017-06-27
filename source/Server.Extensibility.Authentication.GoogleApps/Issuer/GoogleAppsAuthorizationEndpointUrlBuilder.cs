using System;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer
{
    public class GoogleAppsAuthorizationEndpointUrlBuilder : AuthorizationEndpointUrlBuilder<IGoogleAppsConfigurationStore>, IGoogleAppsAuthorizationEndpointUrlBuilder
    {
        public GoogleAppsAuthorizationEndpointUrlBuilder(IGoogleAppsConfigurationStore configurationStore, IUrlEncoder urlEncoder) : base(configurationStore, urlEncoder)
        {
        }

        public override string Build(string requestDirectoryPath, IssuerConfiguration issuerConfiguration, string nonce, string state)
        {
            var url = base.Build(requestDirectoryPath, issuerConfiguration, nonce, state);

            var hd = ConfigurationStore.GetHostedDomain();
            if (!string.IsNullOrWhiteSpace(hd))
            {
                url += $"&hd={hd}";
            }

            return url;
        }
    }
}