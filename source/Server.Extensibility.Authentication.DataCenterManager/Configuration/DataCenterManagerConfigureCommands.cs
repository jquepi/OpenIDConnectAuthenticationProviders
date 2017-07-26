using System;
using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfigureCommands : OpenIdConnectConfigureCommands<IDataCenterManagerConfigurationStore>
    {
        public DataCenterManagerConfigureCommands(ILog log, Lazy<IDataCenterManagerConfigurationStore> configurationStore, Lazy<IWebPortalConfigurationStore> webPortalConfigurationStore) : base(log, configurationStore, webPortalConfigurationStore)
        {
        }

        protected override string ConfigurationSettingsName => "DataCenterManager";

        public override IEnumerable<ConfigureCommandOption> GetOptions()
        {
            foreach (var option in base.GetOptions())
            {
                yield return option;
            }
            yield return new ConfigureCommandOption($"{ConfigurationSettingsName}RoleClaimType=", "Set the RoleClaimType.", v =>
            {
                ConfigurationStore.Value.SetRoleClaimType(v);
                Log.Info($"{ConfigurationSettingsName} RoleClaimType set to: {v}");
            });
        }
    }
}