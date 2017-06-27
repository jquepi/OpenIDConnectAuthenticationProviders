using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.HostServices.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADAuthenticationController : AuthenticationController<AzureADConfigurationStore>
    {
        public AzureADAuthenticationController(
            ILog log,
            AzureADConfigurationStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            IAuthorizationEndpointUrlBuilder urlBuilder,
            IWebPortalConfigurationStore webPortalConfigurationStore) : base(log, configurationStore, identityProviderConfigDiscoverer, urlBuilder, webPortalConfigurationStore)
        {
        }

        [HttpGet]
        [Route("users/authenticate/azureAD")]
        public Task<IActionResult> Authenticate(LoginRedirectLinkRequestModel model)
        {
            return ProcessAuthenticate(model);
        }
    }
}