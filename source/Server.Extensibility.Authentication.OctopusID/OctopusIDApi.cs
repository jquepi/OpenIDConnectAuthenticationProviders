using System;
using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctopusID.Identities;
using Octopus.Server.Extensibility.Authentication.OctopusID.Tokens;
using Octopus.Server.Extensibility.Authentication.OctopusID.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.OctopusID
{
    public class OctopusIDApi : OpenIDConnectModule<OctopusIDUserAuthenticationAction, IOctopusIDConfigurationStore, OctopusIDUserAuthenticatedAction, IOctopusIDAuthTokenHandler, IOctopusIDIdentityCreator>
    {
        public OctopusIDApi(
            IOctopusIDConfigurationStore configurationStore, 
            OctopusIDAuthenticationProvider authenticationProvider,
            Func<WhenEnabledAsyncActionInvoker<OctopusIDUserAuthenticationAction, IOctopusIDConfigurationStore>> authenticateUserActionFactory,
            Func<WhenEnabledAsyncActionInvoker<OctopusIDUserAuthenticatedAction, IOctopusIDConfigurationStore>> userAuthenticatedActionFactory) : base(configurationStore, authenticationProvider)
        {
            Add("POST", authenticationProvider.AuthenticateUri, authenticateUserActionFactory().ExecuteAsync);
            Add("POST", configurationStore.RedirectUri, userAuthenticatedActionFactory().ExecuteAsync);
        }
    }
}