using Newtonsoft.Json;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public class IssuerConfiguration
    {
        public string Issuer  { get; set; }

        [JsonProperty("jwks_uri")]
        public string JwksUri { get; set; }
        
        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }
    }
}