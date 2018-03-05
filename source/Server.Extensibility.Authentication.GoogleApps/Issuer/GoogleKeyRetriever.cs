using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer
{
    public class GoogleKeyRetriever : KeyRetriever<IGoogleAppsConfigurationStore, IKeyJsonParser>, IGoogleKeyRetriever
    {
        public GoogleKeyRetriever(IGoogleAppsConfigurationStore configurationStore, IKeyJsonParser keyParser, ILog log) : base(configurationStore, keyParser, log)
        {
        }
    }
}