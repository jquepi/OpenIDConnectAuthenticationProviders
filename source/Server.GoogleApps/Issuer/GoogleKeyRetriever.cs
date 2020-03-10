using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Certificates;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer
{
    class GoogleKeyRetriever : KeyRetriever<IGoogleAppsConfigurationStore, IKeyJsonParser>, IGoogleKeyRetriever
    {
        public GoogleKeyRetriever(IGoogleAppsConfigurationStore configurationStore, IKeyJsonParser keyParser, ILog log) : base(configurationStore, keyParser, log)
        {
        }
    }
}