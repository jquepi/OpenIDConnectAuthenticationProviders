using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.Extensions;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.Resources;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectAuthenticationProvider<TStore> : IAuthenticationProviderWithGroupSupport 
        where TStore : IOpenIDConnectConfigurationStore
    {
        readonly ILog log;

        protected OpenIDConnectAuthenticationProvider(ILog log, TStore configurationStore)
        {
            this.log = log;
            ConfigurationStore = configurationStore;
        }

        protected TStore ConfigurationStore { get; }

        public abstract string IdentityProviderName { get; }

        public bool IsEnabled => ConfigurationStore.GetIsEnabled() && IsProviderConfigComplete();

        private bool IsProviderConfigComplete()
        {
            var isComplete = true;
            foreach (var reason in ReasonsWhyConfigIsIncomplete())
            {
                log.Warn(reason);
                isComplete = false;
            }
            return isComplete;
        }

        protected abstract IEnumerable<string> ReasonsWhyConfigIsIncomplete();

        public bool SupportsPasswordManagement => false;

        public string AuthenticateUri => $"/api/users/authenticate/{ConfigurationStore.ConfigurationSettingsName}";

        protected abstract string LoginLinkHtml(string siteBaseUri);

        public AuthenticationProviderElement GetAuthenticationProviderElement(string siteBaseUri)
        {
            var authenticationProviderElement = new AuthenticationProviderElement
            {
                Name = IdentityProviderName,
                LinkHtml = LoginLinkHtml(siteBaseUri)
            };
            authenticationProviderElement.Links.Add(AuthenticationProviderElement.AuthenticateLinkName, siteBaseUri + AuthenticateUri);

            return authenticationProviderElement;
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