using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.HostServices.Authentication;
using Octopus.Server.Extensibility.HostServices.Diagnostics;
using Octopus.Server.Extensibility.HostServices.Model;
using Octopus.Server.Extensibility.HostServices.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticatedAction : UserAuthenticatedAction<IGoogleAppsConfigurationStore, IGoogleAuthTokenHandler>
    {
        public GoogleAppsUserAuthenticatedAction(ILog log, IGoogleAuthTokenHandler authTokenHandler, IPrincipalToUserHandler principalToUserHandler, IUserStore userStore, IGoogleAppsConfigurationStore configurationStore, IAuthCookieCreator authCookieCreator, IInvalidLoginTracker loginTracker, ISleep sleep) : base(log, authTokenHandler, principalToUserHandler, userStore, configurationStore, authCookieCreator, loginTracker, sleep)
        {
        }
    }
}