using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.OctopusID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Issuer
{
    public class OctopusIDKeyRetriever : KeyRetriever<IOctopusIDConfigurationStore, IKeyJsonParser>, IOctopusIDKeyRetriever
    {
        public OctopusIDKeyRetriever(IOctopusIDConfigurationStore configurationStore, IKeyJsonParser keyParser, ILog log) : base(configurationStore, keyParser, log)
        {
        }
    }
}