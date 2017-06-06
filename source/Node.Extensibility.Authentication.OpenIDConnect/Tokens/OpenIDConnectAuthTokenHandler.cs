using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens
{
    public abstract class OpenIDConnectAuthTokenHandler<TStore, TRetriever> : IAuthTokenHandler
        where TStore : IOpenIDConnectConfigurationStore
        where TRetriever : ICertificateRetriever
    {
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly TRetriever certificateRetriever;

        readonly ILog log;
        protected readonly TStore ConfigurationStore;

        protected OpenIDConnectAuthTokenHandler(
            ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            TRetriever certificateRetriever)
        {
            this.log = log;
            ConfigurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.certificateRetriever = certificateRetriever;
        }

        public Task<ClaimsPrincipleContainer> GetPrincipalAsync(Dictionary<string, string> requestForm, out string state)
        {
            state = null;
            if (requestForm.ContainsKey("error"))
            {
                var errorDescription = requestForm["error_description"];
                log.Error($"Failed to authenticate user: {errorDescription}");
                return Task.FromResult(new ClaimsPrincipleContainer(errorDescription));
            }

            var accessToken = requestForm["access_token"];
            var idToken = requestForm["id_token"];

            state = requestForm["state"];

            return GetPrincipalFromToken(accessToken, idToken);
        }

        protected virtual void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        { }

        async Task<ClaimsPrincipleContainer> GetPrincipalFromToken(string accessToken, string idToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var issuer = ConfigurationStore.GetIssuer();
            var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

            var certificates = await certificateRetriever.GetCertificatesAsync(issuerConfig);

            var validationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = issuer + "/resources",
                ValidIssuer = issuerConfig.Issuer,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) => !certificates.ContainsKey(identifier) ? null : new[] { new X509SecurityKey(certificates[identifier]) }
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

            // This is where we actually interpret the token, validate it, and pump out a ClaimsPrincipal
            SecurityToken unused;
            var principal = handler.ValidateToken(tokenToValidate, validationParameters, out unused);

            var error = string.Empty;
            DoIssuerSpecificClaimsValidation(principal, out error);
            if (string.IsNullOrEmpty(error))
                return new ClaimsPrincipleContainer(principal);
            else
                return new ClaimsPrincipleContainer(error);
        }

        protected virtual void DoIssuerSpecificClaimsValidation(ClaimsPrincipal principal, out string error)
        {
            error = string.Empty;
        }
    }
}