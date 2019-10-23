using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration
{
    public class GoogleAppsDatabaseInitializer : DatabaseInitializer<GoogleAppsConfiguration>
    {
        public GoogleAppsDatabaseInitializer(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string SingletonId => GoogleAppsConfigurationStore.SingletonId;
    }
}