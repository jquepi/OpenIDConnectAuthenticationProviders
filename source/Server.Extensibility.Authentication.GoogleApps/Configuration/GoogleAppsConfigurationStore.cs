using System.Collections.Generic;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfigurationStore : OpenIdConnectConfigurationStore<GoogleAppsConfiguration>, IGoogleAppsConfigurationStore
    {
        public const string SingletonId = "authentication-googleapps";

        public override string Id => SingletonId;
        public override string ConfigurationSettingsName => "GoogleApps";

        public GoogleAppsConfigurationStore(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public string GetHostedDomain()
        {
            return GetProperty(doc => doc.HostedDomain);
        }

        public void SetHostedDomain(string hostedDomain)
        {
            SetProperty(doc => doc.HostedDomain = hostedDomain);
        }

        public override string ConfigurationSetName => "GoogleApps";

        public override string Description => "GoogleApps authentication settings";

        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }

            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.HostedDomain", GetHostedDomain(), GetIsEnabled(), "Hosted Domain");
        }
    }
}