using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.HostServices.Mapping;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationStore : OpenIdConnectConfigurationWithRoleStore<AzureADConfiguration>, IAzureADConfigurationStore
    {
        public const string SingletonId = "authentication-aad";

        public override string Id => SingletonId;

        public override string ConfigurationSettingsName => "AzureAD";

        public AzureADConfigurationStore(
            IConfigurationStore configurationStore,
            IResourceMappingFactory factory) : base(configurationStore, factory)
        {
        }
    }
}