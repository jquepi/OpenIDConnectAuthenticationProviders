using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADKeyRetriever : KeyRetriever<IAzureADConfigurationStore, IKeyJsonParser>, IAzureADKeyRetriever
    {
        public AzureADKeyRetriever(IAzureADConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(configurationStore, keyParser)
        {
        }
    }
}