using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Time;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public abstract class AzureADAuthenticatedController : AuthenticatedController<IAzureADConfigurationStore, AzureADAuthTokenHandler>
    {
        protected AzureADAuthenticatedController(ILog log,
            AzureADAuthTokenHandler authTokenHandler,
            IPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            IAzureADConfigurationStore configurationStore,
            IInvalidLoginTracker loginTracker,
            IUrlEncoder urlEncoder,
            ISleep sleep,
            IClock clock) : base(authTokenHandler, principalToUserResourceMapper, userStore, configurationStore, loginTracker, urlEncoder, sleep, clock)
        {
        }
        
        [HttpPost]
        [Route("users/authenticated/azureAD")]
        public Task<IActionResult> Authenticated()
        {
            return ProcessAuthenticated();
        }
    }
}