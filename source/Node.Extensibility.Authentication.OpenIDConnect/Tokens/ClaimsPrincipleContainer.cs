using System.Security.Claims;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens
{
    public class ClaimsPrincipleContainer
    {
        public ClaimsPrincipleContainer(string error)
        {
            this.error = error;
        }
        public ClaimsPrincipleContainer(ClaimsPrincipal principal)
        {
            this.principal = principal;
        }

        public string error { get; private set; }
        public ClaimsPrincipal principal { get; private set; }
    }
}
