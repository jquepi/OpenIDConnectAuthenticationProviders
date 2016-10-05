using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Nancy;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public abstract class OpenIDConnectAuthTokenHandler<TStore, TRetriever> : IAuthTokenHandler
        where TStore : IOpenIDConnectConfigurationStore
        where TRetriever : ICertificateRetriever
    {
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly TRetriever certificateRetriever;

        protected readonly TStore ConfigurationStore;

        protected OpenIDConnectAuthTokenHandler(
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            TRetriever certificateRetriever)
        {
            ConfigurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.certificateRetriever = certificateRetriever;
        }

        public Task<ClaimsPrincipal> GetPrincipalAsync(Request request, out string state)
        {
            state = null;
            if (request.Form.ContainsKey("error"))
            {
                return null;
            }

            var accessToken = request.Form["access_token"];
            var idToken = request.Form["id_token"];

            state = request.Form["state"];

            return GetPrincipalFromToken(accessToken, idToken);
        }

        protected virtual void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        { }

        async Task<ClaimsPrincipal> GetPrincipalFromToken(string accessToken, string idToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var issuer = ConfigurationStore.GetIssuer();
            var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

            var certificates = await certificateRetriever.GetCertificatesAsync(issuerConfig);

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = issuer + "/resources",
                ValidIssuer = issuerConfig.Issuer,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) => !certificates.ContainsKey(identifier) ? null : new [] { new X509SecurityKey(certificates[identifier]) }
            };

            if (!string.IsNullOrWhiteSpace(ConfigurationStore.GetNameClaimType()))
                validationParameters.NameClaimType = ConfigurationStore.GetNameClaimType();

            var tokenToValidate = accessToken;
            if (string.IsNullOrWhiteSpace(tokenToValidate))
            {
                // if we're validating the id_token then the audience is based on the client_id, not the issuer/resource like access_token
                tokenToValidate = idToken;
                validationParameters.ValidAudience = ConfigurationStore.GetClientId();
            }

            SetIssuerSpecificTokenValidationParameters(validationParameters);

            SecurityToken unused;
            var principal = handler.ValidateToken(tokenToValidate, validationParameters, out unused);

            DoIssuerSpecificClaimsValidation(principal);

            return principal;
        }

        protected virtual void DoIssuerSpecificClaimsValidation(ClaimsPrincipal principal)
        { }
    }
}