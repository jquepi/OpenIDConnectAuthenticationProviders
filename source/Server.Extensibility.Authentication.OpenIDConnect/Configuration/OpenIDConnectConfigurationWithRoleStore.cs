using Nevermore.Contracts;
using Octopus.Data.Storage.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public abstract class OpenIdConnectConfigurationWithRoleStore<TConfiguration> : OpenIdConnectConfigurationStore<TConfiguration>, IOpenIDConnectConfigurationWithRoleStore<TConfiguration>
        where TConfiguration : OpenIDConnectConfigurationWithRole, IId, new()
    {
        protected OpenIdConnectConfigurationWithRoleStore(IConfigurationStore configurationStore) : base(configurationStore)
        {
        }

        public string GetRoleClaimType()
        {
            return GetProperty(doc => doc.RoleClaimType);
        }

        public void SetRoleClaimType(string roleClaimType)
        {
            SetProperty(doc => doc.RoleClaimType = roleClaimType);
        }
    }
}