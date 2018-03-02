using System.Security.Claims;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public class ClaimsPrincipleContainer
    {
        public ClaimsPrincipleContainer(string error)
        {
            this.Error = error;
        }
        public ClaimsPrincipleContainer(ClaimsPrincipal principal, string[] externalRoleIds)
        {
            this.Principal = principal;
            ExternalRoleIds = externalRoleIds;
        }

        public string Error { get; private set; }
        public ClaimsPrincipal Principal { get; private set; }

        /// <summary>
        /// Gets or sets the external Role/Group Ids
        /// </summary>
        public string[] ExternalRoleIds { get; private set; }
    }
}
