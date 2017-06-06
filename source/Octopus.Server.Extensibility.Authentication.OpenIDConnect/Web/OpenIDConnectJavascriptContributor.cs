using System.Collections.Generic;
using System.Linq;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class OpenIDConnectJavascriptContributor<TStore> : IContributesJavascript
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly TStore configurationStore;

        protected OpenIDConnectJavascriptContributor(TStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        public IEnumerable<string> GetJavascriptUris()
        {
            return !configurationStore.GetIsEnabled()
                ? Enumerable.Empty<string>()
                : new[] { $"/areas/users/{JavascriptFilenamePrefix}_auth_provider.js" };
        }

        public abstract string JavascriptFilenamePrefix { get; }
    }
}