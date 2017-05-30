using System.Threading.Tasks;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public interface IIdentityProviderConfigDiscoverer
    {
        Task<IssuerConfiguration> GetConfigurationAsync(string issuer);
    }
}