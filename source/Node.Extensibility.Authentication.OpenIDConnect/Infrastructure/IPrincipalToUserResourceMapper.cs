using System.Security.Claims;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure
{
    public interface IPrincipalToUserResourceMapper
    {
        UserResource MapToUserResource(ClaimsPrincipal principal);
    }
}