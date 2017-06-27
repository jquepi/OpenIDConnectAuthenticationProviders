using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public abstract class OpenIDConnectAuthTokenHandler<TStore, TRetriever> : AuthTokenHandler<TStore, TRetriever>, IAuthTokenHandler
        where TStore : IOpenIDConnectConfigurationStore
        where TRetriever : IKeyRetriever
    {
        readonly ILog log;

        protected OpenIDConnectAuthTokenHandler(ILog log,
            TStore configurationStore,
            IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer,
            TRetriever keyRetriever) : base(configurationStore, identityProviderConfigDiscoverer, keyRetriever)
        {
            this.log = log;
        }

        public Task<ClaimsPrincipleContainer> GetPrincipalAsync(IFormCollection requestForm, out string state)
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
    }
}