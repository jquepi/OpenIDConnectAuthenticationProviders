using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Time;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADAuthenticatedController : AuthenticatedController<IAzureADConfigurationStore, IAzureADAuthTokenHandler>
    {
        public AzureADAuthenticatedController(ILog log,
            IAzureADAuthTokenHandler authTokenHandler,
            IPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            IAzureADConfigurationStore configurationStore,
            IInvalidLoginTracker loginTracker,
            IUrlEncoder urlEncoder,
            ISleep sleep,
            IClock clock) : base(log, authTokenHandler, principalToUserResourceMapper, userStore, configurationStore, loginTracker, urlEncoder, sleep, clock)
        {
        }

        protected override string ProviderName => AzureADAuthenticationProvider.ProviderName;

        [HttpPost]
        [Route(OpenIdConnectConfigurationStore.AuthenticatedTokenBaseUri + "/azureAD")]
        public Task<IActionResult> Authenticated()
        {
            return ProcessAuthenticated();
        }
    }
}