using System.Collections.Generic;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfigurationSettings : OpenIdConnectConfigurationSettings<GoogleAppsConfiguration, GoogleAppsConfigurationResource, IGoogleAppsConfigurationStore>, IGoogleAppsConfigurationSettings
    {
        public GoogleAppsConfigurationSettings(IGoogleAppsConfigurationStore configurationDocumentStore) : base(configurationDocumentStore)
        {
        }

        public override string Id => GoogleAppsConfigurationStore.SingletonId;
        public override string Description => "GoogleApps authentication settings";

        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }

            yield return new ConfigurationValue($"Octopus.{ConfigurationDocumentStore.ConfigurationSettingsName}.HostedDomain", ConfigurationDocumentStore.GetHostedDomain(), ConfigurationDocumentStore.GetIsEnabled(), "Hosted Domain");
        }
    }
}