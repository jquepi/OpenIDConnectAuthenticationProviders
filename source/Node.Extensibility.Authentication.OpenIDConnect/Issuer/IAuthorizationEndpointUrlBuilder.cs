namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer
{
    public interface IAuthorizationEndpointUrlBuilder
    {
        string Build(string requestDirectoryPath, IssuerConfiguration issuerConfiguration, string nonce, string state);
    }
}