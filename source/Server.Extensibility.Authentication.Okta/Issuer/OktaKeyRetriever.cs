using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Issuer
{
    public class OktaKeyRetriever : KeyRetriever<IOktaConfigurationStore, IKeyJsonParser>, IOktaKeyRetriever
    {
        public OktaKeyRetriever(IOktaConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(configurationStore, keyParser)
        {
        }
    }
}