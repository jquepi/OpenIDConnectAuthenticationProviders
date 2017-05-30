using System.Collections.Generic;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsConfigurationStore : OpenIdConnectConfigurationStore<GoogleAppsConfiguration>, IGoogleAppsConfigurationStore
    {
        protected override string SingletonId => "authentication-googleapps";
        public override string ConfigurationSettingsName => "GoogleApps";

        public GoogleAppsConfigurationStore(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public string GetCertificateUri()
        {
            var doc = ConfigurationStore.Get<GoogleAppsConfiguration>(SingletonId);
            return doc?.CertificateUri;
        }

        public void SetCertificateUri(string certificateUri)
        {
            ConfigurationStore.CreateOrUpdate<GoogleAppsConfiguration>(SingletonId, doc => doc.CertificateUri = certificateUri);
        }

        public string GetHostedDomain()
        {
            var doc = ConfigurationStore.Get<GoogleAppsConfiguration>(SingletonId);
            return doc?.HostedDomain;
        }

        public void SetHostedDomain(string hostedDomain)
        {
            ConfigurationStore.CreateOrUpdate<GoogleAppsConfiguration>(SingletonId, doc => doc.HostedDomain = hostedDomain);
        }

        public override string ConfigurationSetName => "GoogleApps";
        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }

            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.CertificateUri", GetCertificateUri(), GetIsEnabled() && GetCertificateUri() != GoogleAppsConfiguration.DefaultCertificateUri, "Certificate Uri");
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.HostedDomain", GetHostedDomain(), GetIsEnabled(), "Hosted Domain");
        }
    }
}