using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class AuthenticationController<TStore> : Controller
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;
        readonly TStore configurationStore;
        readonly IAuthenticationRedirectUrlBuilder redirectUrlBuilder;
        readonly IWebPortalConfigurationStore webPortalConfigurationStore;

        protected AuthenticationController(ILog log,
            TStore configurationStore,
            IAuthenticationRedirectUrlBuilder redirectUrlBuilder,
            IWebPortalConfigurationStore webPortalConfigurationStore)
        {
            this.log = log;
            this.configurationStore = configurationStore;
            this.redirectUrlBuilder = redirectUrlBuilder;
            this.webPortalConfigurationStore = webPortalConfigurationStore;
        }

        protected async Task<IActionResult> ProcessAuthenticate(LoginRedirectLinkRequestModel model)
        {
            if (configurationStore.GetIsEnabled() == false)
            {
                log.Warn($"{configurationStore.ConfigurationSettingsName} user authentication API was called while the provider was disabled.");
                return BadRequest("This authentication provider is disabled.");
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

            return await redirectUrlBuilder.GetAuthenticationRedirectUrl(Response, State.Encode(state), nonce);
        }
    }
}