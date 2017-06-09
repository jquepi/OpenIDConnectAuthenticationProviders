using System.Threading.Tasks;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public interface IIdentityProviderConfigDiscoverer
    {
        Task<IssuerConfiguration> GetConfigurationAsync(string issuer);
    }
}