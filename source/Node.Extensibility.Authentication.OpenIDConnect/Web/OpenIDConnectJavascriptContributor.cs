using System.Collections.Generic;
using System.Linq;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Web
{
    public abstract class OpenIDConnectJavascriptContributor<TStore> : IContributesJavascript, IContributesAngularModules
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly TStore configurationStore;

        protected OpenIDConnectJavascriptContributor(TStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        public IEnumerable<string> GetAngularModuleNames()
        {
            if (!configurationStore.GetIsEnabled())
                return Enumerable.Empty<string>();
            return new [] { "octopusApp.users." + AngularModuleNameSuffix };
        }
        protected abstract string AngularModuleNameSuffix { get; }

        public IEnumerable<string> GetJavascriptUris()
        {
            if (!configurationStore.GetIsEnabled())
                return Enumerable.Empty<string>();
            return new[]
            {
                $"~/areas/users/{JavascriptFilenamePrefix}_users_module.js",
                $"~/areas/users/controllers/{JavascriptFilenamePrefix}_auth_provider_controller.js",
                $"~/areas/users/directives/{JavascriptFilenamePrefix}_auth_provider.js"
            };
        }

        public abstract string JavascriptFilenamePrefix { get; }
    }
}