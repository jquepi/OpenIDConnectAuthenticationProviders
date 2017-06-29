using System;
using System.Collections.Generic;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD
{
    public class AzureADAuthenticationProvider : OpenIDConnectAuthenticationProvider<IAzureADConfigurationStore>
    {
        public const string ProviderName = "Azure AD";
        
        public AzureADAuthenticationProvider(ILog log, IAzureADConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName => ProviderName;
        public override string FilenamePrefix => "azureAD";

        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            var issuer = ConfigurationStore.GetIssuer();
            if (string.IsNullOrWhiteSpace(issuer))
                yield return $"No {IdentityProviderName} issuer specified";
            if (!Uri.IsWellFormedUriString(issuer, UriKind.Absolute))
                yield return $"The {IdentityProviderName} issuer must be an absolute URI (expected format: https://login.microsoftonline.com/[issuer guid])";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
        }
    }
}