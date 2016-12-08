using System.Linq;
using System.Security.Claims;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure
{
    public class PrincipalToUserResourceMapper : IPrincipalToUserResourceMapper
    {
        public UserResource MapToUserResource(ClaimsPrincipal principal)
        {
            var userResource = new UserResource
            {
                DisplayName = principal.Identity.Name,
                EmailAddress = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            };

            if (!string.IsNullOrEmpty(userResource.EmailAddress))
            {
                userResource.Username = userResource.EmailAddress;
            }
            else if (!string.IsNullOrEmpty(userResource.DisplayName))
            {
                userResource.Username = userResource.DisplayName;
            }

            return userResource;
        }
    }
}