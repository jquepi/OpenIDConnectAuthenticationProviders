using System.Collections.Generic;
using System.Reflection;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADStaticContentFolders : IContributesStaticContentFolders
    {
        public IEnumerable<StaticContentEmbeddedResourcesFolder> GetStaticContentFolders()
        {
            var type = typeof(AzureADStaticContentFolders).GetTypeInfo();
            var assembly = type.Assembly;
            return new[] { new StaticContentEmbeddedResourcesFolder("", assembly, type.Namespace + ".Static") };
        }
    }
}