using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Time;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADKeyRetriever : KeyRetriever<IAzureADConfigurationStore, IKeyJsonParser>, IAzureADKeyRetriever
    {
        public AzureADKeyRetriever(IClock clock, IAzureADConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(clock, configurationStore, keyParser)
        {
        }
    }
}