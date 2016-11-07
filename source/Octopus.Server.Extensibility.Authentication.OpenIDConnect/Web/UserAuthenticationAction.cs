using System;
using System.Threading.Tasks;
using Nancy;
using Nancy.Cookies;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class UserAuthenticationAction<TStore> : IAsyncApiAction
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly IAuthorizationEndpointUrlBuilder urlBuilder;

        protected readonly TStore ConfigurationStore;
        protected readonly IApiActionResponseCreator ResponseCreator;

        protected UserAuthenticationAction(
            ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionResponseCreator responseCreator)
        {
            this.log = log;
            ResponseCreator = responseCreator;
            ConfigurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.urlBuilder = urlBuilder;
        }

        public async Task<Response> ExecuteAsync(NancyContext context, IResponseFormatter response)
        {
            if (ConfigurationStore.GetIsEnabled() == false)
            {
                log.Warn($"{ConfigurationStore.ConfigurationSettingsName} user authentication API was called while the provider was disabled.");
                return ResponseCreator.BadRequest(new string[] { "This authentication provider is disabled." });
            }

            if (context.Request.Url.SiteBase.StartsWith("https://", StringComparison.OrdinalIgnoreCase) == false)
                log.Warn($"{ConfigurationStore.ConfigurationSettingsName} user authentication API was called without using https.");

            var postLoginRedirectTo = context.Request.Query["redirectTo"];
            var state = "~/app";
            if (string.IsNullOrWhiteSpace(postLoginRedirectTo) == false)
                state = postLoginRedirectTo;
            var nonce = Nonce.GenerateUrlSafeNonce();

            try
            {
                var issuer = ConfigurationStore.GetIssuer();
                var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);
                var url = urlBuilder.Build(context.Request.DirectoryPath(), issuerConfig, nonce, state);

                return response.AsRedirect(url)
                    .WithCookie(new NancyCookie("s", State.Protect(state), true, false, DateTime.UtcNow.AddMinutes(20)))
                    .WithCookie(new NancyCookie("n", Nonce.Protect(nonce), true, false, DateTime.UtcNow.AddMinutes(20)));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return response.AsRedirect($"{state}?error=Login failed. Please see the Octopus Server logs for more details.");
            }
        }
    }
}