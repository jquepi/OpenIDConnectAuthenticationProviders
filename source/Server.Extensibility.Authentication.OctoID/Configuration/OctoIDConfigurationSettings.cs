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
        public override string Description => "Octopus ID authentication settings";

        public override IEnumerable<IConfigurationValue> GetConfigurationValues()
        {
            var configurationSettingsName = ConfigurationDocumentStore.ConfigurationSettingsName;
            var isEnabled = ConfigurationDocumentStore.GetIsEnabled();

            yield return new ConfigurationValue<bool>($"Octopus.OctopusID.IsEnabled", isEnabled, isEnabled, "Is Enabled");
            yield return new ConfigurationValue<string>($"Octopus.OctopusID.Issuer", ConfigurationDocumentStore.GetIssuer(), isEnabled, "Issuer");
            yield return new ConfigurationValue<string>($"Octopus.OctopusID.ClientId", ConfigurationDocumentStore.GetClientId(), isEnabled, "ClientId", true);
            yield return new ConfigurationValue<bool>($"Octopus.OctopusID.HasClientSecret", !string.IsNullOrWhiteSpace(ConfigurationDocumentStore.GetClientSecret()), ConfigurationDocumentStore.GetIsEnabled(), "Client Secret has been set");
            yield return new ConfigurationValue<bool>($"Octopus.OctopusID.AllowAutoUserCreation", ConfigurationDocumentStore.GetAllowAutoUserCreation(), isEnabled, "Allow auto user creation");
        }
    }
}
