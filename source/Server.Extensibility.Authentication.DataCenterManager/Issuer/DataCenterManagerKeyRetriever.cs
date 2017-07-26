using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Issuer
{
    public class DataCenterManagerKeyRetriever : KeyRetriever<IDataCenterManagerConfigurationStore, IKeyJsonParser>, IDataCenterManagerKeyRetriever
    {
        public DataCenterManagerKeyRetriever(IDataCenterManagerConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(configurationStore, keyParser)
        {
        }
    }
}