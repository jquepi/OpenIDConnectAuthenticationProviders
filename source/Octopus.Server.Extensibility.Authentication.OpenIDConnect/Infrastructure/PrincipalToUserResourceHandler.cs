using System.Linq;
using System.Security.Claims;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure
{
    public class PrincipalToUserResourceHandler : IPrincipalToUserHandler
    {
        public UserResource GetUserResource(ClaimsPrincipal principal)
        {
            var userResource = new UserResource
            {
                DisplayName = principal.Identity.Name,
                EmailAddress = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            };

            return userResource;
        }
    }
}