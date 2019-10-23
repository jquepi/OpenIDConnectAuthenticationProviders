using System;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Identities;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps
{
    public class GoogleAppsApi : OpenIDConnectModule<GoogleAppsUserAuthenticationAction, IGoogleAppsConfigurationStore, GoogleAppsUserAuthenticatedAction, IGoogleAuthTokenHandler, IGoogleAppsIdentityCreator>
    {
        public GoogleAppsApi(
            IGoogleAppsConfigurationStore configurationStore, 
            GoogleAppsAuthenticationProvider authenticationProvider,
            Func<WhenEnabledAsyncActionInvoker<GoogleAppsUserAuthenticationAction, IGoogleAppsConfigurationStore>> authenticateUserActionFactory,
            Func<WhenEnabledAsyncActionInvoker<GoogleAppsUserAuthenticatedAction, IGoogleAppsConfigurationStore>> userAuthenticatedActionFactory) : base(configurationStore, authenticationProvider)
        {
            Add("POST", authenticationProvider.AuthenticateUri, authenticateUserActionFactory().ExecuteAsync);
            Add("POST", configurationStore.RedirectUri, userAuthenticatedActionFactory().ExecuteAsync);
        }
    }
}