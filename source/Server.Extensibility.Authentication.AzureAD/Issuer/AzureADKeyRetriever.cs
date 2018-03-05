using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Issuer
{
    public class AzureADKeyRetriever : KeyRetriever<IAzureADConfigurationStore, IKeyJsonParser>, IAzureADKeyRetriever
    {
        public AzureADKeyRetriever(IAzureADConfigurationStore configurationStore, IKeyJsonParser keyParser, ILog log) : base(configurationStore, keyParser, log)
        {
        }
    }
}