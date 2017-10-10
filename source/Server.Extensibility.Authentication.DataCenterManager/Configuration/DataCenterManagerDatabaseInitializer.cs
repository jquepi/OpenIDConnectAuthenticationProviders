using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerDatabaseInitializer : DatabaseInitializer<DataCenterManagerConfiguration>
    {
        public DataCenterManagerDatabaseInitializer(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string SingletonId => DataCenterManagerConfigurationStore.SingletonId;
    }
}