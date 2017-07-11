using System;
using System.Collections.Generic;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Authenticate;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Diagnostics;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD
{
    public class AzureADAuthenticationProvider : DCMOpenIDConnectAuthenticationProvider<IAzureADConfigurationStore>
    {
        public const string ProviderName = "Azure AD";
        
        public AzureADAuthenticationProvider(
            ILog log,
            IAzureADConfigurationStore configurationStore,
            IAuthenticationRedirectUrlBuilder redirectUrlBuilder,
            INonceChainer nonceChainer,
            IStateChainer stateChainer) : base(log, configurationStore, redirectUrlBuilder, nonceChainer, stateChainer)
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