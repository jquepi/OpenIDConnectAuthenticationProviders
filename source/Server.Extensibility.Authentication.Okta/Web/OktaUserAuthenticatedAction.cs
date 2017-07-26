using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.Okta.Infrastructure;
using Octopus.Server.Extensibility.Authentication.Okta.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.Okta.Web
{
    public class OktaUserAuthenticatedAction : UserAuthenticatedAction<IOktaConfigurationStore, IOktaAuthTokenHandler>
    {
        public OktaUserAuthenticatedAction(
            ILog log,
            IOktaAuthTokenHandler authTokenHandler,
            IOktaPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            IOktaConfigurationStore configurationStore,
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

        protected override string ProviderName => OktaAuthenticationProvider.ProviderName;
    }
}