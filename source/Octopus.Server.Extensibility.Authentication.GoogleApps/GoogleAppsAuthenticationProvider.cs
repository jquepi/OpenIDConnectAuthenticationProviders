using System;
using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps
{
    public class GoogleAppsAuthenticationProvider : OpenIDConnectAuthenticationProvider<IGoogleAppsConfigurationStore>
    {
        public const string ProviderName = "Google Apps";

        public GoogleAppsAuthenticationProvider(ILog log, IGoogleAppsConfigurationStore configurationStore) : base(log, configurationStore)
        {
        }

        public override string IdentityProviderName => ProviderName;

        protected override IEnumerable<string> ReasonsWhyConfigIsIncomplete()
        {
            var issuer = ConfigurationStore.GetIssuer();
            if (string.IsNullOrWhiteSpace(issuer))
                yield return $"No {IdentityProviderName} issuer specified";
            if (!Uri.IsWellFormedUriString(issuer, UriKind.Absolute))
                yield return $"The {IdentityProviderName} issuer must be an absolute URI (expected format: https://accounts.google.com)";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId()))
                yield return $"No {IdentityProviderName} Client ID specified";
            if (string.IsNullOrWhiteSpace(ConfigurationStore.GetHostedDomain()))
                yield return $"No {IdentityProviderName} hosted domain specified";
        }

        protected override string LoginLinkHtml()
        {
            return "<google-auth-provider provider='provider' should-auto-login='shouldAutoLogin' is-submitting='isSubmitting' handle-sign-in-error='handleSignInError'></google-auth-provider>";
        }
    }
}