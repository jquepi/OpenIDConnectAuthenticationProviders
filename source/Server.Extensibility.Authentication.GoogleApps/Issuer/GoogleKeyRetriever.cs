using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer
{
    public class GoogleKeyRetriever : KeyRetriever<IGoogleAppsConfigurationStore, IKeyJsonParser>, IGoogleKeyRetriever
    {
        public GoogleKeyRetriever(IClock clock, IGoogleAppsConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(clock, configurationStore, keyParser)
        {
        }
    }
}