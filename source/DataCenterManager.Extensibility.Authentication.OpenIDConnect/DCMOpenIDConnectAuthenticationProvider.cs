using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect
{
    public abstract class DCMOpenIDConnectAuthenticationProvider<TStore> : OpenIDConnectAuthenticationProvider<TStore>, IDataCenterManagerOpenIDConnectAuthenticationProvider
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly IAuthenticationRedirectUrlBuilder redirectUrlBuilder;
        readonly INonceChainer nonceChainer;
        readonly IStateChainer stateChainer;

        protected DCMOpenIDConnectAuthenticationProvider(ILog log, 
            TStore configurationStore,
            IAuthenticationRedirectUrlBuilder redirectUrlBuilder,
            INonceChainer nonceChainer,
            IStateChainer stateChainer) : base(log, configurationStore)
        {
            this.redirectUrlBuilder = redirectUrlBuilder;
            this.nonceChainer = nonceChainer;
            this.stateChainer = stateChainer;
        }

        public virtual Task<IActionResult> GetAuthenticationRedirectUrl(
            HttpResponse response, 
            string clientId,
            string redirectUrl,
            string state, 
            string nonce)
        {
            var chainedState = stateChainer.Chain(state, clientId, redirectUrl);
            var chainedNonce = nonceChainer.Chain(nonce, Nonce.GenerateUrlSafeNonce());

            return redirectUrlBuilder.GetAuthenticationRedirectUrl(response, chainedState, chainedNonce);
        }
    }

    public interface IDataCenterManagerAuthenticationProvider
    {
        
    }

    public interface IDataCenterManagerOpenIDConnectAuthenticationProvider : IDataCenterManagerAuthenticationProvider
    {
        Task<IActionResult> GetAuthenticationRedirectUrl(HttpResponse response, string clientId, string redirectUrl, string state, string nonce);
    }
}