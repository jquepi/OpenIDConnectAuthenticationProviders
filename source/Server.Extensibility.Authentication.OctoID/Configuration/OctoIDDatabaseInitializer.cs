using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDDatabaseInitializer : DatabaseInitializer<OctoIDConfiguration>
    {
        public OctoIDDatabaseInitializer(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string SingletonId => OctoIDConfigurationStore.SingletonId;
    }
}