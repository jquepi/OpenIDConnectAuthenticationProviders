using Octopus.Data.Storage.User;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticatedAction 
        : UserAuthenticatedAction<IGoogleAppsConfigurationStore, IGoogleAuthTokenHandler>
    {
        public GoogleAppsUserAuthenticatedAction(
            ILog log,
            IGoogleAuthTokenHandler authTokenHandler,
            IPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUserStore userStore,
            IGoogleAppsConfigurationStore configurationStore,
            IApiActionResponseCreator responseCreator,
            IAuthCookieCreator authCookieCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep) 
            : base(log, authTokenHandler, principalToUserResourceMapper, userStore, configurationStore, responseCreator, authCookieCreator, loginTracker, sleep)
        {
        }
    }
}