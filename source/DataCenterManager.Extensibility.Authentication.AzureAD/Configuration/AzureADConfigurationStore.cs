using System.Collections.Generic;
using Octopus.Data.Storage.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.HostServices.Mapping;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationStore : OpenIdConnectConfigurationStore<AzureADConfiguration, AzureADConfigurationResource>, IAzureADConfigurationStore
    {
        public const string SingletonId = "authentication-aad";

        public override string Id => SingletonId;

        public override string ConfigurationSettingsName => "AzureAD";

        public AzureADConfigurationStore(
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

        public override string ConfigurationSetName => "Azure AD";
        public override IEnumerable<ConfigurationValue> GetConfigurationValues()
        {
            foreach (var configurationValue in base.GetConfigurationValues())
            {
                yield return configurationValue;
            }
            yield return new ConfigurationValue($"DataCenterManager.{ConfigurationSettingsName}.RoleClaimType", GetRoleClaimType(), GetIsEnabled() && GetRoleClaimType() != AzureADConfiguration.DefaultRoleClaimType, "Role Claim Type");
        }
    }
}