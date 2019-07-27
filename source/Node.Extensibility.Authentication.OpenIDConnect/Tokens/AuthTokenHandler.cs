using System;
using System.Collections.Generic;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Octopus.Diagnostics;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public abstract class AuthTokenHandler<TStore, TRetriever>
        where TStore : IOpenIDConnectConfigurationStore
        where TRetriever : IKeyRetriever
    {
        readonly IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        readonly TRetriever keyRetriever;
        protected readonly ILog Log;
        protected readonly TStore ConfigurationStore;

        protected AuthTokenHandler(TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            TRetriever keyRetriever,
            ILog log)
        {
            ConfigurationStore = configurationStore;
            this.identityProviderConfigDiscoverer = identityProviderConfigDiscoverer;
            this.keyRetriever = keyRetriever;
            Log = log;
        }

        protected async Task<ClaimsPrincipleContainer> GetPrincipalFromToken(string accessToken, string idToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var issuer = ConfigurationStore.GetIssuer();
            var issuerConfig = await identityProviderConfigDiscoverer.GetConfigurationAsync(issuer);

            var keys = !string.IsNullOrWhiteSpace(issuerConfig.JwksUri) ? await keyRetriever.GetKeysAsync(issuerConfig) : new Dictionary<string, AsymmetricSecurityKey>();

            var validationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidAudience = issuer + "/resources",
                ValidateIssuer = true,
                ValidIssuer = issuerConfig.Issuer,
                ValidateIssuerSigningKey = true
            };

            if (string.IsNullOrWhiteSpace(issuerConfig.JwksUri))
            {
                if (ConfigurationStore is IOpenIDConnectWithClientSecretConfigurationStore clientSecretStore)
                {
                    var clientSecret = clientSecretStore.GetClientSecret();
                    var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret));

                    validationParameters.SignatureValidator =
                        delegate (string token, TokenValidationParameters parameters)
                        {
                            // we have to do this manually, because the JwtSecurityToken seems to modify the header
                            // and the validation then fails
                            var parts = token.Split('.');
                            var encodedData = parts[0] + "." + parts[1];
                            var byteArray = Encoding.UTF8.GetBytes(encodedData);
                            var hashValue = hmac.ComputeHash(byteArray);
                            var compiledSignature = Base64UrlEncoder.Encode(hashValue);
                            
                            //Validate the incoming jwt signature against the header and payload of the token
                            if (compiledSignature != parts[2])
                            {
                                throw new Exception("Token signature validation failed.");
                            }

                            return new JwtSecurityToken(token);
                        };
                }
                else
                {
                    throw new InvalidOperationException("Identity provider is configured to use client secrets, but isn't configured to support that for this provider.");
                }
            }
            else
            {
                validationParameters.IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    if (!keys.ContainsKey(identifier))
                    {
                        Log.InfoFormat("No signing key found for kid {0}", identifier);
                        return null;
                    }

                    return new[] {keys[identifier]};
                };
            }

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
                if (string.IsNullOrWhiteSpace(issuerConfig.JwksUri))
                {
                    Log.WarnFormat("Unable to verify authentication token using clientSecret");
                    throw;
                }
                // we using x509 certs if there's a JwksUri, and they may have been cycled so signal the code below
                // to reload the keys and retry
                signatureError = true;
            }

            // If we receive an invalid signature, it might be because the provider has cycled their keys.
            // So flush the key cache, reload the keys, and try once more.
            if (signatureError)
            {
                try
                {
                    Log.InfoFormat("Unable to locate signature key, attempting reload. Currently cached kids are {0}", string.Join(", ", keys.Keys));
                    keys = await keyRetriever.GetKeysAsync(issuerConfig, true);
                    principal = handler.ValidateToken(tokenToValidate, validationParameters, out unused);
                }
                catch (SecurityTokenInvalidSignatureException)
                {
                    // we still didn't find the right key. Log the kids we have
                    Log.WarnFormat("Unable to locate signature key. Cached kids are {0}", string.Join(", ", keys.Keys));
                    throw;
                }
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