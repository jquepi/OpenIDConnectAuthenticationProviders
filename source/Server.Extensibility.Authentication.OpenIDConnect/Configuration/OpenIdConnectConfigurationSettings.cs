using System.Collections.Generic;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.HostServices.Mapping;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class OpenIdConnectConfigurationSettings<TConfiguration, TResource, TDocumentStore> : ExtensionConfigurationSettings<TConfiguration, TResource, TDocumentStore>
        where TConfiguration : OpenIDConnectConfiguration, new()
        where TResource : ExtensionConfigurationResource
        where TDocumentStore : IOpenIDConnectConfigurationStore<TConfiguration>
    {
        protected OpenIdConnectConfigurationSettings(TDocumentStore configurationDocumentStore) : base(configurationDocumentStore)
        {
        }

        public override string ConfigurationSetName => ConfigurationDocumentStore.ConfigurationSettingsName;

        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            var configurationSettingsName = ConfigurationDocumentStore.ConfigurationSettingsName;
            var isEnabled = ConfigurationDocumentStore.GetIsEnabled();

            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.IsEnabled", isEnabled.ToString(), isEnabled, "Is Enabled");
            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.Issuer", ConfigurationDocumentStore.GetIssuer(), isEnabled, "Issuer");
            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.ClientId", ConfigurationDocumentStore.GetClientId(), isEnabled, "ClientId", true);
            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.Scope", ConfigurationDocumentStore.GetScope(), isEnabled && ConfigurationDocumentStore.GetScope() != OpenIDConnectConfiguration.DefaultScope, "Scope");
            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.RedirectUri", ConfigurationDocumentStore.RedirectUri, isEnabled, "RedirectUri");
            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.NameClaimType", ConfigurationDocumentStore.GetNameClaimType(), isEnabled && ConfigurationDocumentStore.GetNameClaimType() != OpenIDConnectConfiguration.DefaultNameClaimType, "Name Claim Type");
            yield return new ConfigurationValue($"Octopus.{configurationSettingsName}.AllowAutoUserCreation", ConfigurationDocumentStore.GetAllowAutoUserCreation().ToString(), isEnabled, "Allow auto user creation");
        }

        public override void BuildMappings(IResourceMappingsBuilder builder)
        {
            builder.Map<TResource, TConfiguration>();
        }
    }
}