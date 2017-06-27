using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(IDictionary<string, object> requestForm, out string state);
    }
}