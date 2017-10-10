using System;
using System.Collections.Generic;
using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public class OpenIdConnectConfigurationStore
    {
        public const string AuthenticatedTokenBaseUri = "/users/authenticatedToken";
    }

    public abstract class OpenIdConnectConfigurationStore<TConfiguration> : ExtensionConfigurationStore<TConfiguration, TConfiguration>, IOpenIDConnectConfigurationStore, IHasConfigurationSettings
        where TConfiguration : OpenIDConnectConfiguration, IId, new()
    {
        public abstract string ConfigurationSettingsName { get; }

        protected readonly IConfigurationStore ConfigurationStore;

        protected OpenIdConnectConfigurationStore(IConfigurationStore configurationStore) : base(configurationStore)
        {
            ConfigurationStore = configurationStore;
        }

        protected override TConfiguration MapToResource(TConfiguration doc)
        {
            return doc;
        }

        protected override TConfiguration MapFromResource(TConfiguration resource)
        {
            return resource;
        }

        public string GetIssuer()
        {
            return GetProperty(doc => doc.Issuer);
        }

        public void SetIssuer(string issuer)
        {
            Guid issuerAsGuid;
            if (Guid.TryParse(issuer, out issuerAsGuid))
                throw new ArgumentException($"The {ConfigurationSettingsName} issuer must be an absolute URI and not a GUID (please refer to the Octopus auth-provider's documentation for details)");
            if (!Uri.IsWellFormedUriString(issuer, UriKind.Absolute))
                throw new ArgumentException($"The {ConfigurationSettingsName} issuer must be an absolute URI (please refer to the Octopus auth-provider's documentation for details)");
            SetProperty(doc => doc.Issuer = issuer);
        }

        public string GetResponseType()
        {
            return GetProperty(doc => doc.ResponseType);
        }

        public void SetResponseType(string responseType)
        {
            SetProperty(doc => doc.ResponseType = responseType);
        }

        public string GetResponseMode()
        {
            return GetProperty(doc => doc.ResponseMode);
        }

        public void SetResponseMode(string responseMode)
        {
            SetProperty(doc => doc.ResponseMode = responseMode);
        }

        public string GetClientId()
        {
            return GetProperty(doc => doc.ClientId);
        }

        public void SetClientId(string clientId)
        {
            SetProperty(doc => doc.ClientId = clientId);
        }

        public string GetScope()
        {
            return GetProperty(doc => doc.Scope);
        }

        public void SetScope(string scope)
        {
            SetProperty(doc => doc.Scope = scope);
        }

        public string GetNameClaimType()
        {
            return GetProperty(doc => doc.NameClaimType);
        }

        public void SetNameClaimType(string nameClaimType)
        {
            SetProperty(doc => doc.NameClaimType = nameClaimType);
        }

        public string GetLoginLinkLabel()
        {
            return GetProperty(doc => doc.LoginLinkLabel);
        }

        public void SetLoginLinkLabel(string loginLinkLabel)
        {
            SetProperty(doc => doc.LoginLinkLabel = loginLinkLabel);
        }

        public bool GetAllowAutoUserCreation()
        {
            return GetProperty(doc => doc.AllowAutoUserCreation.GetValueOrDefault(true));
        }

        public void SetAllowAutoUserCreation(bool allowAutoUserCreation)
        {
            SetProperty(doc => doc.AllowAutoUserCreation = allowAutoUserCreation);
        }

        public string RedirectUri => $"{OpenIdConnectConfigurationStore.AuthenticatedTokenBaseUri}/{ConfigurationSettingsName}";

        public override string Id => ConfigurationSettingsName;

        public override string Description => string.Empty;

        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
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