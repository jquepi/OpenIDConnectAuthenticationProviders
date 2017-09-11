using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Identities;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Infrastructure;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Web
{
    public class DataCenterManagerUserAuthenticatedAction : UserAuthenticatedAction<IDataCenterManagerConfigurationStore, IDataCenterManagerAuthTokenHandler, IDataCenterManagerIdentityCreator>
    {
        public DataCenterManagerUserAuthenticatedAction(
            ILog log,
            IDataCenterManagerAuthTokenHandler authTokenHandler,
            IDataCenterManagerPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            IDataCenterManagerConfigurationStore configurationStore,
            IApiActionResponseCreator responseCreator,
            IAuthCookieCreator authCookieCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep,
            IDataCenterManagerIdentityCreator identityCreator) :
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
                identityCreator)
        {
        }

        protected override string ProviderName => DataCenterManagerAuthenticationProvider.ProviderName;
    }
}