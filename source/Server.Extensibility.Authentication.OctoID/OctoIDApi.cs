using System;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctoID.Identities;
using Octopus.Server.Extensibility.Authentication.OctoID.Tokens;
using Octopus.Server.Extensibility.Authentication.OctoID.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.OctoID
{
    public class OctoIDApi : OpenIDConnectModule<OctoIDUserAuthenticationAction, IOctoIDConfigurationStore, OctoIDUserAuthenticatedAction, IOctoIDAuthTokenHandler, IOctoIDIdentityCreator>
    {
        public OctoIDApi(
            IOctoIDConfigurationStore configurationStore, 
            OctoIDAuthenticationProvider authenticationProvider,
            Func<WhenEnabledAsyncActionInvoker<OctoIDUserAuthenticationAction, IOctoIDConfigurationStore>> authenticateUserActionFactory,
            Func<WhenEnabledAsyncActionInvoker<OctoIDUserAuthenticatedAction, IOctoIDConfigurationStore>> userAuthenticatedActionFactory) : base(configurationStore, authenticationProvider)
        {
            Add("POST", authenticationProvider.AuthenticateUri, authenticateUserActionFactory().ExecuteAsync);
            Add("POST", configurationStore.RedirectUri, userAuthenticatedActionFactory().ExecuteAsync);
        }
    }
}