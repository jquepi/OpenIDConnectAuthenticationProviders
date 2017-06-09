using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens;

using System.Threading.Tasks;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens
{
    public interface IGoogleAuthTokenHandler : IAuthTokenHandler
    {
        new Task<ClaimsPrincipleContainer> GetPrincipalAsync(dynamic requestForm, out string state);
    }
}