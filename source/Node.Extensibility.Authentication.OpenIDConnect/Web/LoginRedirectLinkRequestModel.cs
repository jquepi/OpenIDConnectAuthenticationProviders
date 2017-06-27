namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Web
{
    public class LoginRedirectLinkRequestModel
    {
        public string ApiAbsUrl { get; set; }
        public string RedirectAfterLoginTo { get; set; }
    }
}