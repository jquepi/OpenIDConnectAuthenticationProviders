using System;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Identities;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.AzureAD.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.AzureAD
{
    public class AzureADApi : OpenIDConnectModule<AzureADUserAuthenticationAction, IAzureADConfigurationStore, AzureADUserAuthenticatedAction, IAzureADAuthTokenHandler, IAzureADIdentityCreator>
    {
        public AzureADApi(
            IAzureADConfigurationStore configurationStore, 
            AzureADAuthenticationProvider authenticationProvider,
            Func<WhenEnabledAsyncActionInvoker<AzureADUserAuthenticationAction, IAzureADConfigurationStore>> authenticateUserActionFactory,
            Func<WhenEnabledAsyncActionInvoker<AzureADUserAuthenticatedAction, IAzureADConfigurationStore>> userAuthenticatedActionFactory) : base(configurationStore, authenticationProvider)
        {
            Add("POST", authenticationProvider.AuthenticateUri, authenticateUserActionFactory().ExecuteAsync);
            Add("POST", configurationStore.RedirectUri, userAuthenticatedActionFactory().ExecuteAsync);
        }
    }
}