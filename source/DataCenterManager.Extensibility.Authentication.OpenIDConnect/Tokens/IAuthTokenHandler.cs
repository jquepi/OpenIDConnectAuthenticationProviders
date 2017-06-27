using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public interface IAuthTokenHandler
    {
        Task<ClaimsPrincipleContainer> GetPrincipalAsync(IFormCollection requestForm, out string state);
    }
}