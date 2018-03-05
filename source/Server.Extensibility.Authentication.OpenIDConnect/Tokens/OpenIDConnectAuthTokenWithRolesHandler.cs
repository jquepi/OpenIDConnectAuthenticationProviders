using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public abstract class OpenIDConnectAuthTokenWithRolesHandler<TStore, TRetriever> : OpenIDConnectAuthTokenHandler<TStore, TRetriever>
        where TStore : IOpenIDConnectConfigurationWithRoleStore
        where TRetriever : IKeyRetriever
    {
        protected OpenIDConnectAuthTokenWithRolesHandler(ILog log, TStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, TRetriever keyRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
        }

        protected override void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationStore.GetRoleClaimType()))
                validationParameters.RoleClaimType = ConfigurationStore.GetRoleClaimType();
        }

        protected override string[] GetProviderGroupIds(ClaimsPrincipal principal)
        {
            var roleClaimType = ConfigurationStore.GetRoleClaimType();

            if (string.IsNullOrWhiteSpace(roleClaimType))
                return new string[0];

            // the groups Ids consist of external Role and Group identifiers. We always load ClaimTypes.Role claims
            // as external identifiers, and then also based on a custom claim specified by the provider.
            var groups = principal.FindAll(ClaimTypes.Role)
                .Concat(principal.FindAll(roleClaimType))
                .Select(c => c.Value).ToArray();

            return groups;
        }
    }
}