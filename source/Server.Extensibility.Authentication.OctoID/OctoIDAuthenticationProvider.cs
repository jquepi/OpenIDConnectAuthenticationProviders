using System;
using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID
{
    public class OctoIDAuthenticationProvider : OpenIDConnectAuthenticationProvider<IOctoIDConfigurationStore>
    {
        public const string ProviderName = "Octopus ID";

        public OctoIDAuthenticationProvider(ILog log, IOctoIDConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName => ProviderName;
        public override string FilenamePrefix => "octoId";

        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            var issuer = ConfigurationStore.GetIssuer();
            if (string.IsNullOrWhiteSpace(issuer))
                yield return $"No {IdentityProviderName} issuer specified";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
        }
    }
}
