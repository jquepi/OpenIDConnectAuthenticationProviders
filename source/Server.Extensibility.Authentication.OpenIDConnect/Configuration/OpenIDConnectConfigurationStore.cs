using System;
using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.HostServices.Mapping;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class OpenIdConnectConfigurationStore<TConfiguration> : ExtensionConfigurationStore<TConfiguration>, IOpenIDConnectConfigurationStore<TConfiguration>
        where TConfiguration : OpenIDConnectConfiguration, IId, new()
    {
        public abstract string ConfigurationSettingsName { get; }

        protected OpenIdConnectConfigurationStore(
            IConfigurationStore configurationStore, 
            IResourceMappingFactory factory) : base(configurationStore, factory)
        {
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

        public bool GetAllowAutoUserCreation()
        {
            return GetProperty(doc => doc.AllowAutoUserCreation.GetValueOrDefault(true));
        }

        public void SetAllowAutoUserCreation(bool allowAutoUserCreation)
        {
            SetProperty(doc => doc.AllowAutoUserCreation = allowAutoUserCreation);
        }

        public string RedirectUri => $"/api/users/authenticatedToken/{ConfigurationSettingsName}";
    }
}