using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect;

namespace Octopus.Server.Extensibility.Authentication.AzureAD
{
    public class AzureADAuthenticationProvider : OpenIDConnectAuthenticationProvider<IAzureADConfigurationStore>
    {
        public AzureADAuthenticationProvider(IAzureADConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public override string IdentityProviderName => "Azure AD";

        protected override bool IsProviderConfigComplete()
        {
            return !string.IsNullOrWhiteSpace(ConfigurationStore.GetIssuer()) &&
                !string.IsNullOrWhiteSpace(ConfigurationStore.GetClientId());
        }

        protected override string LoginLinkHtml(string siteBaseUri)
        {
            return $"<div style=\"width: 244px;margin:auto;height:40px;border-radius: 2px;box-shadow: 0 1px 1px 0 rgba(0, 0, 0, 0.24), 0 0 1px 0 rgba(0, 0, 0, 0.12);background-color: #000000;\" class=\"ng-scope\"><a href='{{{{authenticateLink}}}}' style=\"color: #ffffff;\"><img src=\"{siteBaseUri}/images/microsoft_signin_buttons/microsoft-logo.svg\" style=\"margin-left: 10px; margin-right: 10px; margin-top: -2px;\"><div style=\"margin-top: 10px;display: inline-block;margin-left: 10px;\">Sign in with Microsoft</div></a></div>";
        }
    }
}