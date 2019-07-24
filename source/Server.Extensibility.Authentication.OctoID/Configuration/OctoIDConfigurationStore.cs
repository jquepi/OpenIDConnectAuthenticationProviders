using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfigurationStore : OpenIDConnectWithClientSecretConfigurationStore<OctoIDConfiguration>, IOctoIDConfigurationStore
    {
        public const string SingletonId = "authentication-octoid";

        public override string Id => SingletonId;

        public override string ConfigurationSettingsName => "OctoID";

        public OctoIDConfigurationStore(
            IConfigurationStore configurationStore) : base(configurationStore)
        {
        }
   }
}