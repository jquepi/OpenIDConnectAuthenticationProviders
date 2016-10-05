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
            return $"<div class='text-center'><a href='{{{{authenticateLink}}}}'><img src=\"{siteBaseUri}/images/microsoft_signin_buttons/sign-in-with-microsoft-light.png\"/></a></div>";
        }
    }
}