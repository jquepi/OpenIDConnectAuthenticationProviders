using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Server.Extensibility.HostServices.Authentication;
using Octopus.Server.Extensibility.HostServices.Model;
using Octopus.Server.Extensibility.HostServices.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticatedAction : UserAuthenticatedAction<IGoogleAppsConfigurationStore, IGoogleAuthTokenHandler>
    {
        public GoogleAppsUserAuthenticatedAction(IGoogleAuthTokenHandler authTokenHandler, IPrincipalToUserHandler principalToUserHandler, IUserStore userStore, IGoogleAppsConfigurationStore configurationStore, IAuthCookieCreator authCookieCreator, IApiActionResponseCreator responseCreator, IInvalidLoginTracker loginTracker, ISleep sleep) : base(authTokenHandler, principalToUserHandler, userStore, configurationStore, authCookieCreator, responseCreator, loginTracker, sleep)
        {
        }
    }
}