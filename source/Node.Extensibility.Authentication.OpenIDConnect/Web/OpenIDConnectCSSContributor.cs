using System.Collections.Generic;
using System.Linq;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class OpenIDConnectCSSContributor<TStore> : IContributesCSS
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly TStore configurationStore;

        protected OpenIDConnectCSSContributor(TStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        public IEnumerable<string> GetCSSUris()
        {
            if (!configurationStore.GetIsEnabled())
                return Enumerable.Empty<string>();
            return new[] { "~/styles/" + CSSFilename + ".css"};
        }

        public abstract string CSSFilename { get; }
    }
}