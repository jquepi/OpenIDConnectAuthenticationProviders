using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfigurationStore : OpenIDConnectWithClientSecretConfigurationStore<OctoIDConfiguration>, IOctoIDConfigurationStore
    {
        public const string SingletonId = "authentication-octoid";

        public override string Id => SingletonId;

        public override string ConfigurationSettingsName => "Octopus ID";

        public OctoIDConfigurationStore(
            IConfigurationStore configurationStore) : base(configurationStore)
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
   }
}