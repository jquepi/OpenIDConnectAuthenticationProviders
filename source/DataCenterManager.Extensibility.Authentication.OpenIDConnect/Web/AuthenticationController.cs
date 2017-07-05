using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class AuthenticationController<TStore> : Controller
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;
        readonly TStore configurationStore;
        readonly IAuthenticationRedirectUrlBuilder redirectUrlBuilder;

        protected AuthenticationController(ILog log,
            TStore configurationStore,
            IAuthenticationRedirectUrlBuilder redirectUrlBuilder)
        {
            this.log = log;
            this.configurationStore = configurationStore;
            this.redirectUrlBuilder = redirectUrlBuilder;
        }

        protected async Task<IActionResult> ProcessAuthenticate(LoginRedirectLinkRequestModel model)
        {
            if (configurationStore.GetIsEnabled() == false)
            {
                log.Warn($"{configurationStore.ConfigurationSettingsName} user authentication API was called while the provider was disabled.");
                return BadRequest("This authentication provider is disabled.");
            }

            var state = model.RedirectAfterLoginTo;

            return await redirectUrlBuilder.GetAuthenticationRedirectUrl(Response, state);
        }
    }
}