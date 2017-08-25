using Nancy;
using Octopus.Server.Extensibility.Authentication.Extensions;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Identities;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectModule<TAuthenticationAction, TStore, TAuthenticatedAction, TAuthTokenHandler, TIdentityCreator> : NancyModule
        where TStore : IOpenIDConnectConfigurationStore
        where TAuthenticationAction : UserAuthenticationAction<TStore>
        where TAuthTokenHandler : IAuthTokenHandler
        where TAuthenticatedAction : UserAuthenticatedAction<TStore, TAuthTokenHandler, TIdentityCreator>
        where TIdentityCreator : IIdentityCreator
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