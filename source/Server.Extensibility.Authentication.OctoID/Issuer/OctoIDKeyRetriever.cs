using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Issuer
{
    public class OctoIDKeyRetriever : KeyRetriever<IOctoIDConfigurationStore, IKeyJsonParser>, IOctoIDKeyRetriever
    {
        public OctoIDKeyRetriever(IOctoIDConfigurationStore configurationStore, IKeyJsonParser keyParser, ILog log) : base(configurationStore, keyParser, log)
        {
        }
    }
}