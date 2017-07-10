using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADStaticContentFolders : OpenIDConnectStaticContentFolders
    {
        public override IEnumerable<StaticContentEmbeddedResourcesFolder> GetStaticContentFolders()
        {
            var type = typeof(AzureADStaticContentFolders).GetTypeInfo();
            var assembly = type.Assembly;
            return base.GetStaticContentFolders().Union(new[] { new StaticContentEmbeddedResourcesFolder("", assembly, type.Namespace + ".Static") });
        }
    }
}