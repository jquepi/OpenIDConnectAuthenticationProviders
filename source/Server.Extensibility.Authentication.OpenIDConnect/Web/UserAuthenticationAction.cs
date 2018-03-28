using System;
using System.Threading.Tasks;
using Nancy;
using Nancy.Cookies;
using Newtonsoft.Json;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;

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
        readonly IAuthenticationConfigurationStore authenticationConfigurationStore;

        protected UserAuthenticationAction(
            ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionResponseCreator responseCreator,
            IApiActionModelBinder modelBinder,
            IAuthenticationConfigurationStore authenticationConfigurationStore)
        {
            this.log = log;
            ResponseCreator = responseCreator;
            this.modelBinder = modelBinder;
            this.authenticationConfigurationStore = authenticationConfigurationStore;
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

            var state = model.State;
            var redirectTo = state.RedirectAfterLoginTo;
            if (string.IsNullOrWhiteSpace(redirectTo))
                redirectTo = "/";

            var whitelist = authenticationConfigurationStore.GetTrustedRedirectUrls();

            if (!Requests.IsLocalUrl(redirectTo, whitelist))
            {
                log.WarnFormat("Prevented potential Open Redirection attack on an authentication request, to the non-local url {0}", redirectTo);
                return ResponseCreator.BadRequest("Request not allowed, due to potential Open Redirection attack");
            }

            var nonce = Nonce.GenerateUrlSafeNonce();

            try
            {
                var issuer = ConfigurationStore.GetIssuer();
                var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

                var stateData = JsonConvert.SerializeObject(state);
                var url = urlBuilder.Build(model.ApiAbsUrl, issuerConfig, nonce, stateData);

                return ResponseCreator.AsOctopusJson(response, new LoginRedirectLinkResponseModel {ExternalAuthenticationUrl = url})
                    .WithCookie(new NancyCookie(UserAuthConstants.OctopusStateCookieName, State.Protect(stateData), true, false, DateTime.UtcNow.AddMinutes(20)))
                    .WithCookie(new NancyCookie(UserAuthConstants.OctopusNonceCookieName, Nonce.Protect(nonce), true, false, DateTime.UtcNow.AddMinutes(20)));
            }
            catch (JsonSerializationException je)
            {
                log.Error(je, "Invalid state passed to server when initiating login");
                return ResponseCreator.BadRequest(je.Message);
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
                return ResponseCreator.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ResponseCreator.BadRequest($"{state}?error=Login failed. Please see the Octopus Server logs for more details.");
            }
        }
    }
}