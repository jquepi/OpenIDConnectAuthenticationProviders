using System.Threading.Tasks;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer
{
    public interface IIdentityProviderConfigDiscoverer
    {
        Task<IssuerConfiguration> GetConfigurationAsync(string issuer);
    }
}