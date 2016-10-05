using System.Security.Claims;
using System.Threading.Tasks;
using Nancy;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipal> GetPrincipalAsync(Request request, out string state);
    }
}