using Microsoft.AspNetCore.Mvc;
using Octopus.Data.Storage.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web.api;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web.api
{
    [Route("api/users/authentication/azureAD/configuration")]
    public class AzureADConfigurationController : ConfigurationController<IConfigurationStore, AzureADConfiguration>
    {
        public AzureADConfigurationController(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string ConfigurationId => AzureADConfigurationStore.SingletonId;
    }
}