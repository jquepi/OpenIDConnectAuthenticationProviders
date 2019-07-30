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
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }
            yield return new ConfigurationValue<bool>($"Octopus.{ConfigurationDocumentStore.ConfigurationSettingsName}.HasClientSecret", !string.IsNullOrWhiteSpace(ConfigurationDocumentStore.GetClientSecret()), ConfigurationDocumentStore.GetIsEnabled(), "Client Secret has been set");
        }
    }
}
