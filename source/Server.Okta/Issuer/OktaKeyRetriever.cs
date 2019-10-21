using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates;

namespace Octopus.Server.Extensibility.Authentication.Okta.Issuer
{
    public class OktaKeyRetriever : KeyRetriever<IOktaConfigurationStore, IKeyJsonParser>, IOktaKeyRetriever
    {
        public OktaKeyRetriever(IOktaConfigurationStore configurationStore, IKeyJsonParser keyParser, ILog log) : base(configurationStore, keyParser, log)
        {
        }
    }
}