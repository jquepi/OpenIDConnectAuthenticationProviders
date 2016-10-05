using System.Security.Claims;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure
{
    public interface IPrincipalToUserHandler
    {
        UserResource GetUserResource(ClaimsPrincipal principal);
    }
}