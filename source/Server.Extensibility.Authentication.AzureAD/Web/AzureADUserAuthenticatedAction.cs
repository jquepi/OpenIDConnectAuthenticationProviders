using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Infrastructure;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADUserAuthenticatedAction : UserAuthenticatedAction<IAzureADConfigurationStore, IAzureADAuthTokenHandler>
    {
        public AzureADUserAuthenticatedAction(
            ILog log,
            IAzureADAuthTokenHandler authTokenHandler,
            IAzureADPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            IAzureADConfigurationStore configurationStore,
            IApiActionResponseCreator responseCreator,
            IAuthCookieCreator authCookieCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep, 
            IClock clock) :
            base(
                log,
                authTokenHandler,
                principalToUserResourceMapper,
                userStore,
                configurationStore,
                responseCreator,
                authCookieCreator,
                loginTracker,
                sleep,
                clock)
        {
        }

        protected override string ProviderName => AzureADAuthenticationProvider.ProviderName;
    }
}