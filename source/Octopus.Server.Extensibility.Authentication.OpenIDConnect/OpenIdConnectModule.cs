using Nancy;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Contracts.Authentication;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectModule<TAuthenticationAction, TStore, TAuthenticatedAction, TAuthTokenHandler> : NancyModule
        where TStore : IOpenIDConnectConfigurationStore
        where TAuthenticationAction : UserAuthenticationAction<TStore>
        where TAuthTokenHandler : IAuthTokenHandler
        where TAuthenticatedAction : UserAuthenticatedAction<TStore, TAuthTokenHandler>
    {
        protected OpenIDConnectModule(TStore configurationStore, IAuthenticationProvider authenticationProvider)
        {
            //if (!configurationStore.GetIsEnabled())
            //    return;

            //Get[authenticationProvider.AuthenticateUri, true] = async (_, token) => await scope.Resolve<TAuthenticationAction>().ExecuteAsync(Context, Response);
            //Post[$"/api/users/authenticatedToken/{authenticationProvider.IdentityProviderName}", true] = async (_, token) => await scope.Resolve<TAuthenticatedAction>().ExecuteAsync(Context, Response);
        }
    }
}