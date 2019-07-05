using Octopus.Data.Storage.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfigurationStore : OpenIdConnectConfigurationStore<OctoIDConfiguration>, IOctoIDConfigurationStore
    {
        public const string SingletonId = "authentication-octoid";

        public override string Id => SingletonId;

        public override string ConfigurationSettingsName => "OctoID";

        public OctoIDConfigurationStore(
            IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public string GetUsernameClaimType()
        {
            return GetProperty(doc => doc.UsernameClaimType);
        }

        public void SetUsernameClaimType(string usernameClaimType)
        {
            SetProperty(doc => doc.UsernameClaimType = usernameClaimType);
        }
    }
}