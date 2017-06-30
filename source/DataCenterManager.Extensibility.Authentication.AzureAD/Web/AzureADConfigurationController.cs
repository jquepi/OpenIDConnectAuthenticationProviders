using Microsoft.AspNetCore.Mvc;
using Octopus.Data.Storage.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Web
{
    [Route("users/authentication/azureAD/configuration")]
    public class AzureADConfigurationController : ConfigurationController<IConfigurationStore, AzureADConfiguration>
    {
        public AzureADConfigurationController(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        protected override string ConfigurationId => AzureADConfigurationStore.Id;
    }
}