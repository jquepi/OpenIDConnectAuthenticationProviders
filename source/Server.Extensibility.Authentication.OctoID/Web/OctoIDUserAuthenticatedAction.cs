using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctoID.Identities;
using Octopus.Server.Extensibility.Authentication.OctoID.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OctoID.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.HostServices.Web;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Web
{
    public class OctoIDUserAuthenticatedAction : UserAuthenticatedAction<IOctoIDConfigurationStore, IOctoIDAuthTokenHandler, IOctoIDIdentityCreator>
    {
        public OctoIDUserAuthenticatedAction(
            ILog log,
            IOctoIDAuthTokenHandler authTokenHandler,
            IOctoIDPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            IOctoIDConfigurationStore configurationStore,
            IAuthCookieCreator authCookieCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep,
            IOctoIDIdentityCreator identityCreator,
            IClock clock, IUrlEncoder encoder) :
            base(
                log,
                authTokenHandler,
                principalToUserResourceMapper,
                userStore,
                configurationStore,
                authCookieCreator,
                loginTracker,
                sleep,
                identityCreator, 
                clock, 
                encoder)
        {
        }

        protected override string ProviderName => OctoIDAuthenticationProvider.ProviderName;
    }
}