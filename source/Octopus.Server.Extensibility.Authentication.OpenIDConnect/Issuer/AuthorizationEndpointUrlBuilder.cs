using Nancy.Helpers;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using System;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public abstract class AuthorizationEndpointUrlBuilder<TStore> : IAuthorizationEndpointUrlBuilder
        where TStore : IOpenIDConnectConfigurationStore
    {
        protected readonly TStore ConfigurationStore;

        protected AuthorizationEndpointUrlBuilder(TStore configurationStore)
        {
            ConfigurationStore = configurationStore;
        }

        public virtual string Build(string siteBaseUri, IssuerConfiguration issuerConfiguration, string nonce, string state)
        {
            if (issuerConfiguration == null)
                throw new ArgumentException("issuerConfiguration is required", nameof(issuerConfiguration));

            var issuerEndpoint = issuerConfiguration.AuthorizationEndpoint;
            var clientId = ConfigurationStore.GetClientId();
            var scope = ConfigurationStore.GetScope();
            var responseType = ConfigurationStore.GetResponseType();
            var responseMode = ConfigurationStore.GetResponseMode();
            var redirectUri = siteBaseUri + ConfigurationStore.RedirectUri;

            var urlPathEncode = HttpUtility.UrlEncode(state);

            var url = $"{issuerEndpoint}?client_id={clientId}&scope={scope}&response_type={responseType}&response_mode={responseMode}&nonce={nonce}&redirect_uri={redirectUri}&state={urlPathEncode}";

            return url;
        }
    }
}