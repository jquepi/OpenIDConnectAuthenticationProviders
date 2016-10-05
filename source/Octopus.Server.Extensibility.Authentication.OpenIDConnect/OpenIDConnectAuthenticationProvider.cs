using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Extensions.Contracts.Authentication;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Resources;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectAuthenticationProvider<TStore> : IAuthenticationProviderWithGroupSupport 
        where TStore : IOpenIDConnectConfigurationStore
    {
        protected OpenIDConnectAuthenticationProvider(TStore configurationStore)
        {
            ConfigurationStore = configurationStore;
        }

        protected TStore ConfigurationStore { get; }

        public abstract string IdentityProviderName { get; }

        public bool IsEnabled => ConfigurationStore.GetIsEnabled() && IsProviderConfigComplete();

        protected abstract bool IsProviderConfigComplete();

        public bool SupportsPasswordManagement => false;

        public string AuthenticateUri => $"/api/users/authenticate/{ConfigurationStore.ConfigurationSettingsName}";

        protected abstract string LoginLinkHtml(string siteBaseUri);

        public AuthenticationProviderElement GetAuthenticationProviderElement(string siteBaseUri)
        {
            return new AuthenticationProviderElement
            {
                Name = IdentityProviderName,
                AuthenticateUri = siteBaseUri + AuthenticateUri,
                LinkHtml = LoginLinkHtml(siteBaseUri)
            };
        }

        public AuthenticationProviderThatSupportsGroups GetGroupLookupElement()
        {
            return new AuthenticationProviderThatSupportsGroups
            {
                Name = IdentityProviderName,
                IsRoleBased = true,
                SupportsGroupLookup = false
            };
        }
    }
}