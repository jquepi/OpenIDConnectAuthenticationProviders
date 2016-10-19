using Octopus.Data.Storage.User;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticatedAction : UserAuthenticatedAction<IGoogleAppsConfigurationStore, IGoogleAuthTokenHandler>
    {
        public GoogleAppsUserAuthenticatedAction(ILog log, IGoogleAuthTokenHandler authTokenHandler, IPrincipalToUserHandler principalToUserHandler, IUserStore userStore, IGoogleAppsConfigurationStore configurationStore, IAuthCookieCreator authCookieCreator, IInvalidLoginTracker loginTracker, ISleep sleep) : base(log, authTokenHandler, principalToUserHandler, userStore, configurationStore, authCookieCreator, loginTracker, sleep)
        {
        }
    }
}