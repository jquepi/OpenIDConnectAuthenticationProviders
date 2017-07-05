using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect
{
    public abstract class DCMOpendIDConnectAuthenticationProvider<TStore> : OpenIDConnectAuthenticationProvider<TStore>, IDataCenterManagerOpenIDConnectAuthenticationProvider
        where TStore : IOpenIDConnectConfigurationStore
    {
        protected DCMOpendIDConnectAuthenticationProvider(ILog log, TStore configurationStore) : base(log, configurationStore)
        {
        }

        public abstract Task<IActionResult> GetAuthenticationRedirectUrl(HttpResponse response, string state);
    }

    public interface IDataCenterManagerAuthenticationProvider
    {
        
    }

    public interface IDataCenterManagerOpenIDConnectAuthenticationProvider : IDataCenterManagerAuthenticationProvider
    {
        Task<IActionResult> GetAuthenticationRedirectUrl(HttpResponse response, string state);
    }
}