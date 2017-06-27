using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Issuer
{
    public class DataCenterManagerKeyRetriever : KeyRetriever<IDataCenterManagerConfigurationStore, IKeyJsonParser>, IDataCenterManagerKeyRetriever
    {
        public DataCenterManagerKeyRetriever(IClock clock, IDataCenterManagerConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(clock, configurationStore, keyParser)
        {
        }
    }
}