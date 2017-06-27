using System;
using System.Threading.Tasks;
using Nancy;
using Nancy.Cookies;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.HostServices.Web;

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
        readonly IApiActionModelBinder modelBinder;
        readonly IWebPortalConfigurationStore webPortalConfigurationStore;

        protected UserAuthenticationAction(
            ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionResponseCreator responseCreator,
            IApiActionModelBinder modelBinder,
            IWebPortalConfigurationStore webPortalConfigurationStore)
        {
            this.log = log;
            ResponseCreator = responseCreator;
            this.modelBinder = modelBinder;
            this.webPortalConfigurationStore = webPortalConfigurationStore;
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

            var model = modelBinder.Bind<LoginRedirectLinkRequestModel>(context);

            var state = model.RedirectAfterLoginTo;
            if (string.IsNullOrWhiteSpace(state))
                state = "/";

            var whitelist = webPortalConfigurationStore.GetTrustedRedirectUrls();

            if (!Requests.IsLocalUrl(state, whitelist))
            {
                log.WarnFormat("Prevented potential Open Redirection attack on an authentication request, to the non-local url {0}", state);
                return ResponseCreator.BadRequest("Request not allowed, due to potential Open Redirection attack");
            }

            var nonce = Nonce.GenerateUrlSafeNonce();

            try
            {
                var issuer = ConfigurationStore.GetIssuer();
                var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

                var url = urlBuilder.Build(model.ApiAbsUrl, issuerConfig, nonce, state);

                return ResponseCreator.AsOctopusJson(response, new LoginRedirectLinkResponseModel { ExternalAuthenticationUrl = url })
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