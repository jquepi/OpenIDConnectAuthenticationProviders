using System;
using System.Collections.Generic;
using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Configuration
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
            Guid issuerAsGuid;
            if (Guid.TryParse(issuer, out issuerAsGuid))
                throw new ArgumentException($"The {ConfigurationSettingsName} issuer must be an absolute URI and not a GUID (please refer to the Octopus auth-provider's documentation for details)");
            if (!Uri.IsWellFormedUriString(issuer, UriKind.Absolute))
                throw new ArgumentException($"The {ConfigurationSettingsName} issuer must be an absolute URI (please refer to the Octopus auth-provider's documentation for details)");
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

        public bool GetAllowAutoUserCreation()
        {
            var doc = ConfigurationStore.Get<TConfiguration>(SingletonId);
            return doc?.AllowAutoUserCreation.GetValueOrDefault(true) ?? true;
        }

        public void SetAllowAutoUserCreation(bool allowAutoUserCreation)
        {
            ConfigurationStore.CreateOrUpdate<TConfiguration>(SingletonId, doc => doc.AllowAutoUserCreation = allowAutoUserCreation);
        }

        public string RedirectUri => $"/users/authenticatedToken/{ConfigurationSettingsName}";

        public abstract string ConfigurationSetName { get; }
        public virtual IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.IsEnabled", GetIsEnabled().ToString(), GetIsEnabled(), "Is Enabled");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.Issuer", GetIssuer(), GetIsEnabled(), "Issuer");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.ClientId", GetClientId(), GetIsEnabled(), "ClientId", true);
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.ResponseType", GetResponseType(), GetIsEnabled() && GetResponseType() != OpenIDConnectConfiguration.DefaultResponseType, "Response Type");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.ResponseMode", GetResponseMode(), GetIsEnabled() && GetResponseMode() != OpenIDConnectConfiguration.DefaultResponseMode, "Response Mode");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.Scope", GetScope(), GetIsEnabled() && GetScope() != OpenIDConnectConfiguration.DefaultScope, "Scope");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.RedirectUri", RedirectUri, GetIsEnabled(), "RedirectUri");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.NameClaimType", GetNameClaimType(), GetIsEnabled() && GetNameClaimType() != OpenIDConnectConfiguration.DefaultNameClaimType, "Name Claim Type");
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.LoginLinkLabel", GetLoginLinkLabel(), false);
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.AllowAutoUserCreation", GetAllowAutoUserCreation().ToString(), GetIsEnabled(), "Allow auto user creation");
        }
    }
}