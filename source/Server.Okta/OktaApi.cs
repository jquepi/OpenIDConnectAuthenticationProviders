using System;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.Okta.Identities;
using Octopus.Server.Extensibility.Authentication.Okta.Tokens;
using Octopus.Server.Extensibility.Authentication.Okta.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.Okta
{
    public class OktaApi : OpenIDConnectModule<OktaUserAuthenticationAction, IOktaConfigurationStore, OktaUserAuthenticatedAction, IOktaAuthTokenHandler, IOktaIdentityCreator>
    {
        public OktaApi(
            IOktaConfigurationStore configurationStore, 
            OktaAuthenticationProvider authenticationProvider,
            Func<WhenEnabledAsyncActionInvoker<OktaUserAuthenticationAction, IOktaConfigurationStore>> authenticateUserActionFactory,
            Func<WhenEnabledAsyncActionInvoker<OktaUserAuthenticatedAction, IOktaConfigurationStore>> userAuthenticatedActionFactory) : base(configurationStore, authenticationProvider)
        {
            Add("POST", authenticationProvider.AuthenticateUri, authenticateUserActionFactory().ExecuteAsync);
            Add("POST", configurationStore.RedirectUri, userAuthenticatedActionFactory().ExecuteAsync);
        }
    }
}