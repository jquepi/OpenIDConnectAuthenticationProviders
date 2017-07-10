using System.Collections.Generic;
using System.Reflection;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class OpenIDConnectStaticContentFolders : IContributesStaticContentFolders
    {
        public virtual IEnumerable<StaticContentEmbeddedResourcesFolder> GetStaticContentFolders()
        {
            var type = typeof(OpenIDConnectStaticContentFolders).GetTypeInfo();
            var assembly = type.Assembly;
            return new[] { new StaticContentEmbeddedResourcesFolder("", assembly, type.Namespace + ".Static") };
        }
    }
}