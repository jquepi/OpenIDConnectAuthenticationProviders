using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.Okta.Issuer
{
    public class OktaKeyRetriever : KeyRetriever<IOktaConfigurationStore, IKeyJsonParser>, IOktaKeyRetriever
    {
        public OktaKeyRetriever(IClock clock, IOktaConfigurationStore configurationStore, IKeyJsonParser keyParser) : base(clock, configurationStore, keyParser)
        {
        }
    }
}