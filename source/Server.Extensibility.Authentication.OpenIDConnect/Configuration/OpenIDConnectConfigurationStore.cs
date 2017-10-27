using System;
using System.Collections.Generic;
using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.HostServices.Mapping;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class OpenIdConnectConfigurationStore<TConfiguration> : ExtensionConfigurationStore<TConfiguration, TConfiguration>, IOpenIDConnectConfigurationStore
        where TConfiguration : OpenIDConnectConfiguration, IId, new()
    {
        public abstract string ConfigurationSettingsName { get; }

        protected OpenIdConnectConfigurationStore(
            IConfigurationStore configurationStore, 
            IResourceMappingFactory factory) : base(configurationStore, factory)
        {
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

        public string RedirectUri => $"/api/users/authenticatedToken/{ConfigurationSettingsName}";

        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
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
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.AllowAutoUserCreation", GetAllowAutoUserCreation().ToString(), GetIsEnabled(), "Allow auto user creation");
        }
    }
}