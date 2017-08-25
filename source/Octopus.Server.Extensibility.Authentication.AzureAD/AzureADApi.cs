using System;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Identities;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.AzureAD.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
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
            Post[authenticationProvider.AuthenticateUri, true] = async (_, token) => await authenticateUserActionFactory().ExecuteAsync(Context, Response);
            Post[configurationStore.RedirectUri, true] = async (_, token) => await userAuthenticatedActionFactory().ExecuteAsync(Context, Response);
        }
    }
}