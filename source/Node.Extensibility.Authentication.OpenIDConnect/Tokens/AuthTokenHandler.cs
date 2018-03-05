using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public abstract class AuthTokenHandler<TStore, TRetriever>
        where TStore : IOpenIDConnectConfigurationStore
        where TRetriever : IKeyRetriever
    {
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly TRetriever keyRetriever;
        protected readonly TStore ConfigurationStore;

        protected AuthTokenHandler(TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            TRetriever keyRetriever)
        {
            ConfigurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.keyRetriever = keyRetriever;
        }

        protected async Task<ClaimsPrincipleContainer> GetPrincipalFromToken(string accessToken, string idToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var issuer = ConfigurationStore.GetIssuer();
            var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

            var keys = await keyRetriever.GetKeysAsync(issuerConfig);

            var validationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidAudience = issuer + "/resources",
                ValidateIssuer = true,
                ValidIssuer = issuerConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) => !keys.ContainsKey(identifier) ? null : new[] { keys[identifier] }
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

            var signatureError = false;
            ClaimsPrincipal principal = null;
            try
            {
                principal = handler.ValidateToken(tokenToValidate, validationParameters, out unused);
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                signatureError = true;
            }

            // If we receive an invalid signature, it might be because the provider has recylced their keys.
            // So reflush the key cache, reload the keys, and try once more.
            if (signatureError)
            {
                keys = await keyRetriever.GetKeysAsync(issuerConfig, true);
                principal = handler.ValidateToken(tokenToValidate, validationParameters, out unused);
            }

            var error = string.Empty;
            DoIssuerSpecificClaimsValidation(principal, out error);

            if (string.IsNullOrWhiteSpace(error))
                return new ClaimsPrincipleContainer(principal, GetProviderGroupIds(principal));

            return new ClaimsPrincipleContainer(error);
        }

        protected virtual void SetIssuerSpecificTokenValidationParameters(TokenValidationParameters validationParameters)
        { }

        protected virtual void DoIssuerSpecificClaimsValidation(ClaimsPrincipal principal, out string error)
        {
            error = string.Empty;
        }

        protected virtual string[] GetProviderGroupIds(ClaimsPrincipal principal)
        {
            return new string[0];
        }
    }
}