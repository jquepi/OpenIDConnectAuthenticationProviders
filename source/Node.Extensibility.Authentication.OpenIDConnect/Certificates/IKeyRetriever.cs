using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public interface IKeyRetriever
    {
        Task<IDictionary<string, AsymmetricSecurityKey>> GetKeysAsync(IssuerConfiguration issuer);
    }
}