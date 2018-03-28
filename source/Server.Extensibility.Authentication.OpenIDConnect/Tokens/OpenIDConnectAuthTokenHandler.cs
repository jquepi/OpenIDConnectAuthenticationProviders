using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Node.Extensibility.Authentication.Resources;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public abstract class OpenIDConnectAuthTokenHandler<TStore, TRetriever> : AuthTokenHandler<TStore, TRetriever>, IAuthTokenHandler
        where TStore : IOpenIDConnectConfigurationStore
        where TRetriever : IKeyRetriever
    {
        protected OpenIDConnectAuthTokenHandler(
            ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            TRetriever keyRetriever) : base(configurationStore, identityProviderConfigDiscoverer, keyRetriever, log)
        {
        }

        public Task<ClaimsPrincipleContainer> GetPrincipalAsync(IDictionary<string, object> requestForm, out LoginState state)
        {
            state = null;
            if (requestForm.ContainsKey("error"))
            {
                var errorDescription = requestForm["error_description"] as string;
                Log.Error($"Failed to authenticate user: {errorDescription}");
                return Task.FromResult(new ClaimsPrincipleContainer(errorDescription));
            }

            string accessToken = null;
            if (requestForm.ContainsKey("access_token"))
            {
                accessToken = requestForm["access_token"] as string;
            }
            
            string idToken = null;
            if (requestForm.ContainsKey("id_token"))
            {
                idToken = requestForm["id_token"] as string;
            }

            try
            {
                var stateData = requestForm["state"] as string;
                state = JsonConvert.DeserializeObject<LoginState>(stateData);
            }
            catch (JsonSerializationException je)
            {
                Log.Error(je, "Invalid state returned to server during login");
                throw;
            }

            return GetPrincipalFromToken(accessToken, idToken);
        }
    }
}