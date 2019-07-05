using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfiguration : OpenIDConnectConfiguration
    {
        public static string DefaultUsernameClaimType = "username";

        public OctoIDConfiguration() : base("OctopusID", "Octopus Deploy", "1.0")
        {
            Id = OctoIDConfigurationStore.SingletonId;
            UsernameClaimType = DefaultUsernameClaimType;
            Scope = DefaultScope;
        }

        public string UsernameClaimType { get; set; }

        
        public string ClientSecret { get; set; }
    }
}