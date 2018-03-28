using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Node.Extensibility.Authentication.Resources;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(IDictionary<string, object> requestForm, out LoginState state);
    }
}