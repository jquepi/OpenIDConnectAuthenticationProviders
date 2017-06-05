using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Tokens
{
    public class AzureADAuthTokenHandler : OpenIDConnectAuthTokenHandler<IAzureADConfigurationStore, ICertificateRetriever>, IAzureADAuthTokenHandler
    {
        public AzureADAuthTokenHandler(ILog log, IAzureADConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, ICertificateRetriever certificateRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, certificateRetriever)
        {
        }

        protected override void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationStore.GetRoleClaimType()))
                validationParameters.RoleClaimType = ConfigurationStore.GetRoleClaimType();
        }
    }
}