using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADAuthenticationController : AuthenticationController<IAzureADConfigurationStore>
    {
        public AzureADAuthenticationController(
            ILog log,
            IAzureADConfigurationStore configurationStore,
            IAuthenticationRedirectUrlBuilder redirectUrlBuilder,
            IWebPortalConfigurationStore webPortalConfigurationStore) : base(log, configurationStore, redirectUrlBuilder, webPortalConfigurationStore)
        {
        }

        [HttpPost]
        [Route("users/authenticate/azureAD")]
        public Task<IActionResult> Authenticate(LoginRedirectLinkRequestModel model)
        {
            return ProcessAuthenticate(model);
        }
    }
}