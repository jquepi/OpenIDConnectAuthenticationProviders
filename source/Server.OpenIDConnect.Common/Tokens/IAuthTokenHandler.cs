using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(IDictionary<string, string> requestForm, out string stateString);
    }
}