namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public interface IAuthorizationEndpointUrlBuilder
    {
        string Build(string siteBaseUri, IssuerConfiguration issuerConfiguration, string nonce, string state);
    }
}