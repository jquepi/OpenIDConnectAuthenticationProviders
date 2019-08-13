using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Configuration
{
    public class OctopusIDDatabaseInitializer : DatabaseInitializer<OctopusIDConfiguration>
    {
        public OctopusIDDatabaseInitializer(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string SingletonId => OctopusIDConfigurationStore.SingletonId;
    }
}