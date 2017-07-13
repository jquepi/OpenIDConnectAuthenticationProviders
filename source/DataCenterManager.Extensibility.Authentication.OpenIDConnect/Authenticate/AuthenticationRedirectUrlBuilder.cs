using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate
{
    public class AuthenticationRedirectUrlBuilder<TStore> : IAuthenticationRedirectUrlBuilder
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;
        readonly TStore configurationStore;
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly IAuthorizationEndpointUrlBuilder urlBuilder;
        readonly IWebPortalConfigurationStore webPortalConfigurationStore;

        public AuthenticationRedirectUrlBuilder(ILog log,
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

        public async Task<IActionResult> GetAuthenticationRedirectUrl(HttpResponse response, string state, string nonce)
        {
            try
            {
                var issuer = configurationStore.GetIssuer();
                var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

                var url = urlBuilder.Build(webPortalConfigurationStore.GetPublicBaseUrl(), issuerConfig, nonce, state);

                response.Cookies.Append(UserAuthConstants.DCMStateCookieName, State.Protect(state), new CookieOptions { Secure = false, HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(20) });
                response.Cookies.Append(UserAuthConstants.DCMNonceCookieName, Nonce.Protect(nonce), new CookieOptions { Secure = false, HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(20) });

                return new RedirectResult(url);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new BadRequestObjectResult($"{state}?error=Login failed. Please see the Octopus Server logs for more details.");
            }
        }
    }

    public interface IAuthenticationRedirectUrlBuilder
    {
        Task<IActionResult> GetAuthenticationRedirectUrl(HttpResponse response, string state, string nonce);
    }
}