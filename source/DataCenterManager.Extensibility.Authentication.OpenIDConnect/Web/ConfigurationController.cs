using Microsoft.AspNetCore.Mvc;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class ConfigurationController<TStore, TConfiguration> : Controller
        where TStore : IConfigurationStore
        where TConfiguration : OpenIDConnectConfiguration, new()
    {
        readonly TStore configurationStore;

        protected ConfigurationController(TStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        protected abstract string ConfigurationId { get; }

        [HttpGet]
        public TConfiguration Index()
        {
            var configuration = configurationStore.Get<TConfiguration>(ConfigurationId);
            if (configuration == null)
            {
                configuration = new TConfiguration();
                configuration.SetId(ConfigurationId);
            }
            return configuration;
        }

        [HttpPut]
        public void Put([FromBody] TConfiguration config)
        {
            config.SetId(ConfigurationId);
            configurationStore.Update(config);
        }

        [HttpPost]
        public void Post([FromBody] TConfiguration config)
        {
            config.SetId(ConfigurationId);
            configurationStore.Create(config);
        }
    }
}