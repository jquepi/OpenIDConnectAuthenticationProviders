using System;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Identities;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Tokens;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager
{
    public class DataCenterManagerApi : OpenIDConnectModule<DataCenterManagerUserAuthenticationAction, IDataCenterManagerConfigurationStore, DataCenterManagerUserAuthenticatedAction, IDataCenterManagerAuthTokenHandler, IDataCenterManagerIdentityCreator>
    {
        public DataCenterManagerApi(
            IDataCenterManagerConfigurationStore configurationStore, 
            DataCenterManagerAuthenticationProvider authenticationProvider,
            Func<WhenEnabledAsyncActionInvoker<DataCenterManagerUserAuthenticationAction, IDataCenterManagerConfigurationStore>> authenticateUserActionFactory,
            Func<WhenEnabledAsyncActionInvoker<DataCenterManagerUserAuthenticatedAction, IDataCenterManagerConfigurationStore>> userAuthenticatedActionFactory) : base(configurationStore, authenticationProvider)
        {
            Post[authenticationProvider.AuthenticateUri, true] = async (_, token) => await authenticateUserActionFactory().ExecuteAsync(Context, Response);
            Post[configurationStore.RedirectUri, true] = async (_, token) => await userAuthenticatedActionFactory().ExecuteAsync(Context, Response);
        }
    }
}