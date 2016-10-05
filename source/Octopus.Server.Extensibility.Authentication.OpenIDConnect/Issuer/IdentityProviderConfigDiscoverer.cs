using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public class IdentityProviderConfigDiscoverer : IIdentityProviderConfigDiscoverer
    {
        readonly Dictionary<string, IssuerConfiguration> configurations = new Dictionary<string, IssuerConfiguration>();

        public async Task<IssuerConfiguration> GetConfigurationAsync(string issuer)
        {
            if (configurations.ContainsKey(issuer))
                return configurations[issuer];

            IssuerConfiguration configuration;

            using (var client = new HttpClient())
            {
                    var response = await client.GetAsync(issuer + "/.well-known/openid-configuration");
                var content = await response.Content.ReadAsStringAsync();

                configuration = JsonConvert.DeserializeObject<IssuerConfiguration>(content);
            }

            configurations.Add(issuer, configuration);

            return configuration;
        }
    }
}