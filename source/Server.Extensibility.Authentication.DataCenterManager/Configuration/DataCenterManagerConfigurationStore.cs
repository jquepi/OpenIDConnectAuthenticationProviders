using System.Collections.Generic;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfigurationStore : OpenIdConnectConfigurationStore<DataCenterManagerConfiguration>, IDataCenterManagerConfigurationStore
    {
        protected override string SingletonId => "authentication-dcm";
        public override string ConfigurationSettingsName => "DataCenterManager";

        public DataCenterManagerConfigurationStore(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public string GetRoleClaimType()
        {
            var doc = ConfigurationStore.Get<DataCenterManagerConfiguration>(SingletonId);
            return doc?.RoleClaimType;
        }

        public void SetRoleClaimType(string roleClaimType)
        {
            ConfigurationStore.CreateOrUpdate<DataCenterManagerConfiguration>(SingletonId, doc => doc.RoleClaimType = roleClaimType);
        }

        public override string ConfigurationSetName => "Data Center Manager";
        
        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.RoleClaimType", GetRoleClaimType(), GetIsEnabled() && GetRoleClaimType() != DataCenterManagerConfiguration.DefaultRoleClaimType, "Role Claim Type");
        }
    }
}