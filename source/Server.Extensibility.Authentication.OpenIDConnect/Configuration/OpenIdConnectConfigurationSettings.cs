using System.Collections.Generic;
using System.Linq;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Extensions;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.HostServices.Mapping;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class OpenIdConnectConfigurationSettings<TConfiguration, TResource, TDocumentStore> : ExtensionConfigurationSettings<TConfiguration, TResource, TDocumentStore>, ICanBeHidden
        where TConfiguration : OpenIDConnectConfiguration, new()
        where TResource : ExtensionConfigurationResource
        where TDocumentStore : IOpenIDConnectConfigurationStore<TConfiguration>
    {
        protected OpenIdConnectConfigurationSettings(TDocumentStore configurationDocumentStore) : base(configurationDocumentStore)
        {
        }

        public override string ConfigurationSetName => ConfigurationDocumentStore.ConfigurationSettingsName;

        public virtual bool IsHidden => false; 
        
        public override IEnumerable<IConfigurationValue> GetConfigurationValues()
        {
            if (IsHidden)
                return Enumerable.Empty<IConfigurationValue>();
            
            var configurationSettingsName = ConfigurationDocumentStore.ConfigurationSettingsName;
            var isEnabled = ConfigurationDocumentStore.GetIsEnabled();

            return new IConfigurationValue[]
            {
                new ConfigurationValue<bool>($"Octopus.{configurationSettingsName}.IsEnabled", isEnabled, isEnabled, "Is Enabled"),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.Issuer", ConfigurationDocumentStore.GetIssuer(), isEnabled, "Issuer"),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.ClientId", ConfigurationDocumentStore.GetClientId(), isEnabled, "ClientId", true),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.Scope", ConfigurationDocumentStore.GetScope(), isEnabled && ConfigurationDocumentStore.GetScope() != OpenIDConnectConfiguration.DefaultScope, "Scope"),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.RedirectUri", ConfigurationDocumentStore.RedirectUri, isEnabled, "RedirectUri"),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.NameClaimType", ConfigurationDocumentStore.GetNameClaimType(), isEnabled && ConfigurationDocumentStore.GetNameClaimType() != OpenIDConnectConfiguration.DefaultNameClaimType, "Name Claim Type"),
                new ConfigurationValue<bool>($"Octopus.{configurationSettingsName}.AllowAutoUserCreation", ConfigurationDocumentStore.GetAllowAutoUserCreation(), isEnabled, "Allow auto user creation")
            };
        }

        public override void BuildMappings(IResourceMappingsBuilder builder)
        {
            builder.Map<TResource, TConfiguration>();
        }
    }
}