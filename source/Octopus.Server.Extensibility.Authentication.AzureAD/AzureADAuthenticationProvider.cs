using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.Server.Extensibility.Authentication.AzureAD
{
    public class AzureADAuthenticationProvider : OpenIDConnectAuthenticationProvider<IAzureADConfigurationStore>
    {
        public AzureADAuthenticationProvider(ILog log, IAzureADConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName => "Azure AD";

        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetIssuer()))
                yield return $"No {IdentityProviderName} issuer specified";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
        }

        protected override string LoginLinkHtml()
        {
            return "<azure-auth-provider provider='provider' should-auto-login='shouldAutoLogin' is-submitting='isSubmitting' handle-sign-in-error='handleSignInError'></azure-auth-provider>";
        }
    }
}