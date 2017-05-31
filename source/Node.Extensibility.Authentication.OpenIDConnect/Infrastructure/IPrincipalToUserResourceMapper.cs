using System.Security.Claims;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Infrastructure
{
    public interface IPrincipalToUserResourceMapper
    {
        UserResource MapToUserResource(ClaimsPrincipal principal);
    }
}