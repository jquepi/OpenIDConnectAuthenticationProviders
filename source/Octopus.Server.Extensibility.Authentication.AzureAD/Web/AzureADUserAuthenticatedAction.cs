using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Server.Extensibility.HostServices.Authentication;
using Octopus.Server.Extensibility.HostServices.Model;
using Octopus.Server.Extensibility.HostServices.Time;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADUserAuthenticatedAction : UserAuthenticatedAction<IAzureADConfigurationStore, IAzureADAuthTokenHandler>
    {
        public AzureADUserAuthenticatedAction(IAzureADAuthTokenHandler authTokenHandler, IPrincipalToUserHandler principalToUserHandler, IUserStore userStore, IAzureADConfigurationStore configurationStore, IAuthCookieCreator authCookieCreator, IApiActionResponseCreator responseCreator, IInvalidLoginTracker loginTracker, ISleep sleep) : base(authTokenHandler, principalToUserHandler, userStore, configurationStore, authCookieCreator, responseCreator, loginTracker, sleep)
        {
        }
    }
}