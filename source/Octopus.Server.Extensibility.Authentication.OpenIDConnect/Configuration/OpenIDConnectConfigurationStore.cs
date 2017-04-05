using System;
using System.Collections.Generic;
using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class OpenIdConnectConfigurationStore<TConfiguration> : IOpenIDConnectConfigurationStore, IHasConfigurationSettings
        where TConfiguration : OpenIDConnectConfiguration, IId, new()
    {
        protected abstract string SingletonId { get; }
        public abstract string ConfigurationSettingsName { get; }

        protected readonly IConfigurationStore ConfigurationStore;

        protected OpenIdConnectConfigurationStore(IConfigurationStore configurationStore)
        {
            ConfigurationStore = configurationStore;
        }

        public bool GetIsEnabled()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.IsEnabled ?? false;
        }

        public void SetIsEnabled(bool isEnabled)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.IsEnabled = isEnabled);
        }

        public string GetIssuer()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.Issuer;
        }

        public void SetIssuer(string issuer)
        {
            if (!Uri.IsWellFormedUriString(issuer, UriKind.Absolute))
                throw new ArgumentException($"The {ConfigurationSettingsName} issuer must be an absolute URI (expected format: https://login.microsoftonline.com/[issuer guid])");
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.Issuer = issuer);
        }

        public string GetResponseType()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.ResponseType;
        }

        public void SetResponseType(string responseType)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.ResponseType = responseType);
        }

        public string GetResponseMode()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.ResponseMode;
        }

        public void SetResponseMode(string responseMode)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.ResponseMode = responseMode);
        }

        public string GetClientId()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.ClientId;
        }

        public void SetClientId(string clientId)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.ClientId = clientId);
        }

        public string GetScope()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.Scope;
        }

        public void SetScope(string scope)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.Scope = scope);
        }

        public string GetNameClaimType()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.NameClaimType;
        }

        public void SetNameClaimType(string nameClaimType)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.NameClaimType = nameClaimType);
        }

        public string GetLoginLinkLabel()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.LoginLinkLabel;
        }

        public void SetLoginLinkLabel(string loginLinkLabel)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.LoginLinkLabel = loginLinkLabel);
        }

        public string RedirectUri => $"/api/users/authenticatedToken/{ConfigurationSettingsName}";

        public abstract string ConfigurationSetName { get; }
        public virtual IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.IsEnabled", GetIsEnabled().ToString(), GetIsEnabled(), "Is Enabled");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.Issuer", GetIssuer(), GetIsEnabled(), "Issuer");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.ClientId", GetClientId(), GetIsEnabled(), "ClientId", true);
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.ResponseType", GetResponseType(), GetIsEnabled() && GetResponseType() != OpenIDConnectConfiguration.DefaultResponseType, "Response Type");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.ResponseMode", GetResponseMode(), GetIsEnabled() && GetResponseMode() != OpenIDConnectConfiguration.DefaultResponseMode, "Response Mode");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.Scope", GetScope(), GetIsEnabled() && GetScope() != OpenIDConnectConfiguration.DefaultScope, "Scope");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.RedirectUri", RedirectUri, GetIsEnabled(), "RedirectUri");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.NameClaimType", GetNameClaimType(), GetIsEnabled() && GetNameClaimType() != OpenIDConnectConfiguration.DefaultNameClaimType, "Name Claim Type");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.LoginLinkLabel", GetLoginLinkLabel(), false);
        }
    }
}