using System;
using System.Collections.Generic;
using Octopus.Data.Model;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Configuration
{
    class OctopusIDConfigureCommands : OpenIdConnectConfigureCommands<IOctopusIDConfigurationStore>
    {
        public OctopusIDConfigureCommands(
            ILog log,
            Lazy<IOctopusIDConfigurationStore> configurationStore,
            Lazy<IWebPortalConfigurationStore> webPortalConfigurationStore)
            : base(log, configurationStore, webPortalConfigurationStore)
        {
        }

        protected override string ConfigurationSettingsName => "octopusId";

        public override IEnumerable<ConfigureCommandOption> GetOptions()
        {
            foreach (var option in base.GetCoreOptions(hide: true))
            {
                yield return option;
            }
            yield return new ConfigureCommandOption($"{ConfigurationSettingsName}ClientSecret=", "Tell Octopus the shared secret to use for Octopus ID authentication requests.", v =>
            {
                ConfigurationStore.Value.SetClientSecret(v.ToSensitiveString());
                Log.Info($"{ConfigurationSettingsName} ClientSecret set");
            }, hide: true);
        }
    }
}