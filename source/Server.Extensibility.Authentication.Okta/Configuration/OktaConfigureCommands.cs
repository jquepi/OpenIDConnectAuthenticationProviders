using System;
using System.Collections.Generic;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfigureCommands : OpenIdConnectConfigureCommands<IOktaConfigurationStore>
    {
        public OktaConfigureCommands(
            ILog log,
            Lazy<IOktaConfigurationStore> configurationStore,
            Lazy<IWebPortalConfigurationStore> webPortalConfigurationStore)
            : base(log, configurationStore, webPortalConfigurationStore)
        {
        }

        protected override string ConfigurationSettingsName => "okta";

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