using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class DatabaseInitializer<TConfiguration> : ExecuteWhenDatabaseInitializes
        where TConfiguration : OpenIDConnectConfiguration, new()
    {
        readonly IConfigurationStore configurationStore;

        protected DatabaseInitializer(IConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        protected abstract string SingletonId { get; }

        public override void Execute()
        {
            var doc = configurationStore.Get<TConfiguration>(SingletonId);
            if (doc != null)
            {
                // TODO: to cover a dev team edge case during 4.0 Alpha. Can be removed before final release
                if (doc.ConfigurationSchemaVersion != "1.0")
                {
                    doc.ConfigurationSchemaVersion = "1.0";
                    configurationStore.Update(doc);
                }
                return;
            }

            configurationStore.Create(new TConfiguration());
        }
    }
}