using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Identities;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsUserAuthenticatedAction : UserAuthenticatedAction<IGoogleAppsConfigurationStore, IGoogleAuthTokenHandler, IGoogleAppsIdentityCreator>
    {
        public GoogleAppsUserAuthenticatedAction(ILog log,
            IGoogleAuthTokenHandler authTokenHandler, 
            IPrincipalToUserResourceMapper principalToUserResourceMapper, 
            IUpdateableUserStore userStore,
            IGoogleAppsConfigurationStore configurationStore, 
            IApiActionResponseCreator responseCreator, 
            IAuthCookieCreator authCookieCreator, 
            IInvalidLoginTracker loginTracker, 
            ISleep sleep,
            IGoogleAppsIdentityCreator identityCreator) : base(
                log, 
                authTokenHandler, 
                principalToUserResourceMapper, 
                userStore, 
                configurationStore, 
                responseCreator, 
                authCookieCreator, 
                loginTracker, 
                sleep,
                identityCreator)
        {
        }

        protected override string ProviderName => GoogleAppsAuthenticationProvider.ProviderName;
    }
}