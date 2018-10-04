using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.Authentication.Resources;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class UserAuthenticationAction<TStore> : IAsyncApiAction
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly IAuthorizationEndpointUrlBuilder urlBuilder;

        protected readonly TStore ConfigurationStore;
        readonly IApiActionModelBinder modelBinder;
        readonly IAuthenticationConfigurationStore authenticationConfigurationStore;

        protected UserAuthenticationAction(
            ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, 
            IAuthorizationEndpointUrlBuilder urlBuilder,
            IApiActionModelBinder modelBinder,
            IAuthenticationConfigurationStore authenticationConfigurationStore)
        {
            this.log = log;
            this.modelBinder = modelBinder;
            this.authenticationConfigurationStore = authenticationConfigurationStore;
            ConfigurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.urlBuilder = urlBuilder;
        }

        public async Task ExecuteAsync(OctoContext context)
        {
            if (ConfigurationStore.GetIsEnabled() == false)
            {
                log.Warn($"{ConfigurationStore.ConfigurationSettingsName} user authentication API was called while the provider was disabled.");
                context.Response.BadRequest("This authentication provider is disabled.");
                return;
            }

            var model = modelBinder.Bind<LoginRedirectLinkRequestModel>();
            
            // If the login state object was passed use it, otherwise fall back to a safe default:
            //   1. Redirecting to the root of the local web site
            //   2. Setting the Cookie.Secure flag according to the current request
            var state = model.State ?? new LoginState{RedirectAfterLoginTo = "/", UsingSecureConnection = context.Request.IsHttps};
            
            // Prevent Open Redirection attacks by ensuring the redirect after successful login is somewhere we trust (local origin or a trusted remote origin)
            var whitelist = authenticationConfigurationStore.GetTrustedRedirectUrls();
            if (!Requests.IsLocalUrl(state.RedirectAfterLoginTo, whitelist))
            {
                log.WarnFormat("Prevented potential Open Redirection attack on an authentication request, to the non-local url {0}", state.RedirectAfterLoginTo);
                context.Response.BadRequest("Request not allowed, due to potential Open Redirection attack");
                return;
            }

            // Finally, provide the client with the information it requires to initiate the redirect to the external identity provider
            try
            {
                var issuer = ConfigurationStore.GetIssuer();
                var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

                // Use a non-deterministic nonce to prevent replay attacks
                var nonce = Nonce.GenerateUrlSafeNonce();
                
                var stateString = JsonConvert.SerializeObject(state);
                var url = urlBuilder.Build(model.ApiAbsUrl, issuerConfig, nonce, stateString);

                // These cookies are used to validate the data returned from the external identity provider - this prevents tampering
                context.Response.AsOctopusJson(new LoginRedirectLinkResponseModel {ExternalAuthenticationUrl = url})
                    .WithCookie(new OctoCookie {Name = UserAuthConstants.OctopusStateCookieName, Value = State.Protect(stateString), HttpOnly = true, Secure = state.UsingSecureConnection, Expires = DateTime.UtcNow.AddMinutes(20)})
                    .WithCookie(new OctoCookie {Name = UserAuthConstants.OctopusNonceCookieName, Value = Nonce.Protect(nonce), HttpOnly = true, Secure = state.UsingSecureConnection, Expires = DateTime.UtcNow.AddMinutes(20)});
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
                context.Response.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                context.Response.BadRequest("Login failed. Please see the Octopus Server logs for more details.");
            }
        }
    }
}