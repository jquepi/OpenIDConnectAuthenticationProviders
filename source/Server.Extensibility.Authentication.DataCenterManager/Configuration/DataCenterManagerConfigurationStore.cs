using System.Collections.Generic;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.HostServices.Mapping;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfigurationStore : OpenIdConnectConfigurationStore<DataCenterManagerConfiguration>, IDataCenterManagerConfigurationStore
    {
        public const string SingletonId = "authentication-dcm";

        public override string Id => SingletonId;

        public override string ConfigurationSettingsName => "DataCenterManager";

        public DataCenterManagerConfigurationStore(
            IConfigurationStore configurationStore,
            IResourceMappingFactory factory) : base(configurationStore, factory)
        {
        }

        public string GetRoleClaimType()
        {
            return GetProperty(doc => doc.RoleClaimType);
        }

        public void SetRoleClaimType(string roleClaimType)
        {
            SetProperty(doc => doc.RoleClaimType = roleClaimType);
        }

        public override string ConfigurationSetName => "Data Center Manager";

        public override string Description => "Data Center Manager authentication settings";

        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }
            yield return new ConfigurationValue($"Octopus.{ConfigurationSettingsName}.RoleClaimType", GetRoleClaimType(), GetIsEnabled() && GetRoleClaimType() != DataCenterManagerConfiguration.DefaultRoleClaimType, "Role Claim Type");
        }

        public override IResourceMapping GetMapping()
        {
            return ResourceMappingFactory
                .Create<DataCenterManagerConfigurationResource, DataCenterManagerConfiguration>();
        }
    }
}