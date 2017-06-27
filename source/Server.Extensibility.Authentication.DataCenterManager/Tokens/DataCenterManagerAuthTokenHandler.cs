using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;
using Microsoft.IdentityModel.Tokens;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Tokens
{
    public class DataCenterManagerAuthTokenHandler : OpenIDConnectAuthTokenHandler<IDataCenterManagerConfigurationStore, IKeyRetriever>, IDataCenterManagerAuthTokenHandler
    {
        public DataCenterManagerAuthTokenHandler(ILog log, IDataCenterManagerConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }

        protected override void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationStore.GetRoleClaimType()))
                validationParameters.RoleClaimType = ConfigurationStore.GetRoleClaimType();
        }
    }
}