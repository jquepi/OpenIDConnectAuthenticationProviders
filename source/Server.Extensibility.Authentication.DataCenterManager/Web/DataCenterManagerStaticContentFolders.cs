using System.Collections.Generic;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Web
{
    public class DataCenterManagerStaticContentFolders : IContributesStaticContentFolders
    {
        public IEnumerable<StaticContentEmbeddedResourcesFolder> GetStaticContentFolders()
        {
            var type = typeof(DataCenterManagerStaticContentFolders);
            var assembly = type.Assembly;
            return new[] { new StaticContentEmbeddedResourcesFolder("", assembly, type.Namespace + ".Static") };
        }
    }
}