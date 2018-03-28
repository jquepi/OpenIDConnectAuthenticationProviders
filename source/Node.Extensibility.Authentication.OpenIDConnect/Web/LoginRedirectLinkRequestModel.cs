using Octopus.Node.Extensibility.Authentication.Resources;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web
{
    public class LoginRedirectLinkRequestModel
    {
        public string ApiAbsUrl { get; set; }
        public LoginState State { get; set; }
    }
}