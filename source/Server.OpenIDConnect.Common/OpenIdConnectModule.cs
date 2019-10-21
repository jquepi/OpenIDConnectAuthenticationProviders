using Octopus.Server.Extensibility.Authentication.Extensions;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Identities;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectModule<TAuthenticationAction, TStore, TAuthenticatedAction, TAuthTokenHandler, TIdentityCreator> : RegisterEndpoint
        where TStore : IOpenIDConnectConfigurationStore
        where TAuthenticationAction : UserAuthenticationAction<TStore>
        where TAuthTokenHandler : IAuthTokenHandler
        where TAuthenticatedAction : UserAuthenticatedAction<TStore, TAuthTokenHandler, TIdentityCreator>
        where TIdentityCreator : IIdentityCreator
    {
        protected OpenIDConnectModule(TStore configurationStore, IAuthenticationProvider authenticationProvider)
        {
        }
    }
}