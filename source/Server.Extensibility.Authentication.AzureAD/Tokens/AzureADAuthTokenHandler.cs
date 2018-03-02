using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Microsoft.IdentityModel.Tokens;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.AzureAD.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Tokens
{
    public class AzureADAuthTokenHandler : OpenIDConnectAuthTokenHandler<IAzureADConfigurationStore, IAzureADKeyRetriever>, IAzureADAuthTokenHandler
    {
        public AzureADAuthTokenHandler(ILog log, IAzureADConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IAzureADKeyRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }

        protected override void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationStore.GetRoleClaimType()))
                validationParameters.RoleClaimType = ConfigurationStore.GetRoleClaimType();
        }
    }
}