using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(Dictionary<string, string> requestForm, out string state);
    }
}