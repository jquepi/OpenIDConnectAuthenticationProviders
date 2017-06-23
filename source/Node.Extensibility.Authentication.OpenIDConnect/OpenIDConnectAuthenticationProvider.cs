﻿using System.Collections.Generic;
using System.Linq;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.Resources;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectAuthenticationProvider<TStore> : 
        IAuthenticationProviderWithGroupSupport,
        IContributesCSS,
        IContributesJavascript
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
        public abstract string FilenamePrefix { get; }

        public bool IsEnabled => ConfigurationStore.GetIsEnabled() && IsProviderConfigComplete();

        bool IsProviderConfigComplete()
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

        public AuthenticationProviderElement GetAuthenticationProviderElement()
        {
            var authenticationProviderElement = new AuthenticationProviderElement
            {
                Name = IdentityProviderName,
                IdentityType = IdentityType.OAuth
            };
            authenticationProviderElement.Links.Add(AuthenticationProviderElement.AuthenticateLinkName, "~" + AuthenticateUri);

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

        public string[] GetAuthenticationUrls()
        {
            return new[] {AuthenticateUri, ConfigurationStore.RedirectUri};
        }

        public IEnumerable<string> GetCSSUris()
        {
            return !ConfigurationStore.GetIsEnabled()
                ? Enumerable.Empty<string>()
                : new[] { $"~/styles/{FilenamePrefix}.css" };
        }

        public IEnumerable<string> GetJavascriptUris()
        {
            return !ConfigurationStore.GetIsEnabled()
                ? Enumerable.Empty<string>()
                : new[] { $"~/areas/users/{FilenamePrefix}_auth_provider.js" };
        }
    }
}