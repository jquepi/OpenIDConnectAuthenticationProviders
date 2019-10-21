using System.Security.Claims;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public class ClaimsPrincipleContainer
    {
        public ClaimsPrincipleContainer(string error)
        {
            this.Error = error;
        }
        public ClaimsPrincipleContainer(ClaimsPrincipal principal, string[] externalGroupIds)
        {
            this.Principal = principal;
            ExternalGroupIds = externalGroupIds;
        }

        public string Error { get; private set; }
        public ClaimsPrincipal Principal { get; private set; }

        /// <summary>
        /// Gets or sets the external Role/Group Ids
        /// </summary>
        public string[] ExternalGroupIds { get; private set; }
    }
}
