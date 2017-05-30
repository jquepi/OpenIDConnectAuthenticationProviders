using System.Collections.Generic;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationStore : OpenIdConnectConfigurationStore<AzureADConfiguration>, IAzureADConfigurationStore
    {
        protected override string SingletonId => "authentication-aad";
        public override string ConfigurationSettingsName => "AzureAD";

        public AzureADConfigurationStore(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public string GetRoleClaimType()
        {
            var doc = ConfigurationStore.Get<AzureADConfiguration>(SingletonId);
            return doc?.RoleClaimType;
        }

        public void SetRoleClaimType(string roleClaimType)
        {
            ConfigurationStore.CreateOrUpdate<AzureADConfiguration>(SingletonId, doc => doc.RoleClaimType = roleClaimType);
        }

        public override string ConfigurationSetName => "Azure AD";
        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.RoleClaimType", GetRoleClaimType(), GetIsEnabled() && GetRoleClaimType() != AzureADConfiguration.DefaultRoleClaimType, "Role Claim Type");
        }
    }
}