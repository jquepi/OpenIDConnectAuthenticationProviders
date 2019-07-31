namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public interface IOpenIDConnectConfiguration
    {
        string Issuer { get; set; }
        string ClientId { get; set; }
        string Scope { get; set; }
        string NameClaimType { get; set; }
        bool AllowAutoUserCreation { get; set; }
    }
}