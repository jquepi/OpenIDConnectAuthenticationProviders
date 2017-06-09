using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(dynamic requestForm, out string state);
    }
}