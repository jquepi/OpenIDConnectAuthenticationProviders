namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web.Models
{
    public class JwtTokenViewModel
    {
        public JwtTokenViewModel(string redirectUri, string state, string token)
        {
            RedirectUri = redirectUri;
            State = state;
            Token = token;
        }

        public string RedirectUri { get; private set; }
        public string State { get; private set; }
        public string Token { get; private set; }
    }
}