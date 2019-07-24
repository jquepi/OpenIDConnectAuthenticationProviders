using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfiguration : OpenIDConnectConfigurationWithClientSecret
    {
        public OctoIDConfiguration() : base("OctopusID", "Octopus Deploy", "1.0")
        {
            Id = OctoIDConfigurationStore.SingletonId;
            Issuer = "https://account.octopus.com";
            Scope = DefaultScope;
        }
    }
}