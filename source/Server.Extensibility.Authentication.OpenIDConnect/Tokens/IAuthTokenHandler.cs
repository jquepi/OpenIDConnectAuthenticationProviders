using System.Threading.Tasks;
using Nancy;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(Request request, out string state);
    }
}