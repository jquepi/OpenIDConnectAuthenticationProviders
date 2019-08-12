using System;
using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfigureCommands : OpenIdConnectConfigureCommands<IOctoIDConfigurationStore>
    {
        public OctoIDConfigureCommands(
            ILog log,
            Lazy<IOctoIDConfigurationStore> configurationStore,
            Lazy<IWebPortalConfigurationStore> webPortalConfigurationStore)
            : base(log, configurationStore, webPortalConfigurationStore)
        {
        }

        protected override string ConfigurationSettingsName => "octoId";

        public override IEnumerable<ConfigureCommandOption> GetOptions()
        {
            foreach (var option in base.GetOptions())
            {
                yield return option;
            }
            yield return new ConfigureCommandOption($"{ConfigurationSettingsName}ClientSecret=", "Tell Octopus the shared secret to use for Octopus ID authentication requests.", v =>
            {
                ConfigurationStore.Value.SetClientSecret(v);
                Log.Info($"{ConfigurationSettingsName} ClientSecret set");
            });
        }
    }
}