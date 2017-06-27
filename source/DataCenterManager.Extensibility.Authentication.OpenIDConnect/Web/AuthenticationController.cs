using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class AuthenticationController<TStore> : Controller
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;
        readonly TStore configurationStore;
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly IAuthorizationEndpointUrlBuilder urlBuilder;
        readonly IWebPortalConfigurationStore webPortalConfigurationStore;

        protected AuthenticationController(ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            IAuthorizationEndpointUrlBuilder urlBuilder,
            IWebPortalConfigurationStore webPortalConfigurationStore)
        {
            this.log = log;
            this.configurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.urlBuilder = urlBuilder;
            this.webPortalConfigurationStore = webPortalConfigurationStore;
        }

        protected async Task<IActionResult> ProcessAuthenticate(LoginRedirectLinkRequestModel model)
        {
            if (configurationStore.GetIsEnabled() == false)
            {
                log.Warn($"{configurationStore.ConfigurationSettingsName} user authentication API was called while the provider was disabled.");
                return new BadRequestObjectResult("This authentication provider is disabled.");
            }

            var state = model.RedirectAfterLoginTo;
            if (string.IsNullOrWhiteSpace(state))
                state = "/";

            var whitelist = webPortalConfigurationStore.GetTrustedRedirectUrls();

            if (!Requests.IsLocalUrl(state, whitelist))
            {
                log.WarnFormat("Prevented potential Open Redirection attack on an authentication request, to the non-local url {0}", state);
                return new BadRequestObjectResult("Request not allowed, due to potential Open Redirection attack");
            }

            var nonce = Nonce.GenerateUrlSafeNonce();

            try
            {
                var issuer = configurationStore.GetIssuer();
                var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

                var url = urlBuilder.Build(model.ApiAbsUrl, issuerConfig, nonce, state);

                Response.Cookies.Append("s", State.Protect(state), new CookieOptions{ Secure = false, HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(20) });
                Response.Cookies.Append("n", Nonce.Protect(nonce), new CookieOptions{ Secure = false, HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(20) });

                return Json(new LoginRedirectLinkResponseModel {ExternalAuthenticationUrl = url});
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new RedirectResult($"{state}?error=Login failed. Please see the Octopus Server logs for more details.");
            }
        }
    }
}