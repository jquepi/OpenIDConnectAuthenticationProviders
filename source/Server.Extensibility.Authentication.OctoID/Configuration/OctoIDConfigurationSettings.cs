using System.Collections.Generic;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfigurationSettings : OpenIdConnectConfigurationSettings<OctoIDConfiguration, OctoIDConfigurationResource, IOctoIDConfigurationStore>, IOctoIDConfigurationSettings
    {
        public OctoIDConfigurationSettings(IOctoIDConfigurationStore configurationDocumentStore) : base(configurationDocumentStore)
        {
        }

        public override string Id => OctoIDConfigurationStore.SingletonId;

        public override string ConfigurationSetName => "Octopus ID";
        
        public override string Description => "Octopus ID authentication settings";

        public override IEnumerable<IConfigurationValue> GetConfigurationValues()
        {
            var configurationSettingsName = ConfigurationDocumentStore.ConfigurationSettingsName;
            var isEnabled = ConfigurationDocumentStore.GetIsEnabled();

            return new IConfigurationValue[]
            {
                new ConfigurationValue<bool>($"Octopus.{configurationSettingsName}.IsEnabled", isEnabled, isEnabled, "Is Enabled"),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.Issuer", ConfigurationDocumentStore.GetIssuer(), isEnabled, "Issuer"),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.ClientId", ConfigurationDocumentStore.GetClientId(), isEnabled, "ClientId", isSensitive: true),
                new ConfigurationValue<string>($"Octopus.{configurationSettingsName}.ClientSecret", ConfigurationDocumentStore.GetClientSecret(), isEnabled && !string.IsNullOrWhiteSpace(ConfigurationDocumentStore.GetClientSecret()), "Client Secret", isSensitive: true),
                new ConfigurationValue<bool>($"Octopus.{configurationSettingsName}.AllowAutoUserCreation", ConfigurationDocumentStore.GetAllowAutoUserCreation(), isEnabled, "Allow auto user creation")
            };
        }
    }
}
