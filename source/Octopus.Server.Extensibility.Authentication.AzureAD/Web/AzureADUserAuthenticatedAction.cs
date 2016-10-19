using Octopus.Data.Storage.User;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADUserAuthenticatedAction : UserAuthenticatedAction<IAzureADConfigurationStore, IAzureADAuthTokenHandler>
    {
        public AzureADUserAuthenticatedAction(ILog log, IAzureADAuthTokenHandler authTokenHandler, IPrincipalToUserHandler principalToUserHandler, IUserStore userStore, IAzureADConfigurationStore configurationStore, IAuthCookieCreator authCookieCreator, IInvalidLoginTracker loginTracker, ISleep sleep) : base(log, authTokenHandler, principalToUserHandler, userStore, configurationStore, authCookieCreator, loginTracker, sleep)
        {
        }
    }
}