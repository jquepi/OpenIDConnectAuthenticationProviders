using System;
using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;

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
            var issuer = ConfigurationStore.GetIssuer();
            if (string.IsNullOrWhiteSpace(issuer))
                yield return $"No {IdentityProviderName} issuer specified";
            if (!Uri.IsWellFormedUriString(issuer, UriKind.Absolute))
                yield return $"The {IdentityProviderName} issuer must be an absolute URI (expected format: https://login.microsoftonline.com/[issuer guid])";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
        }

        protected override string LoginLinkHtml()
        {
            return "<azure-auth-provider provider='provider' should-auto-login='shouldAutoLogin' is-submitting='isSubmitting' handle-sign-in-error='handleSignInError'></azure-auth-provider>";
        }
    }
}