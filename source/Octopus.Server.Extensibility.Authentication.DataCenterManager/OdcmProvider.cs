using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager
{

    public interface IOdcmConfigurationStore : IOpenIDConnectConfigurationStore
    {
        string GetCertificateUri();
        void SetCertificateUri(string certificateUri);

        string GetHostedDomain();
        void SetHostedDomain(string hostedDomain);
    }

    public class OdcmProvider : OpenIDConnectAuthenticationProvider<IOdcmConfigurationStore>
    {
        public OdcmProvider(ILog log, IOdcmConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName { get; }
        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            throw new NotImplementedException();
        }

        protected override string LoginLinkHtml()
        {
            throw new NotImplementedException();
        }
    }
}
