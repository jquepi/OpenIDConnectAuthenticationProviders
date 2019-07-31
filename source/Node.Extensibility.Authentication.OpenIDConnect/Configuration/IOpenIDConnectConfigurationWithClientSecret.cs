namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public interface IOpenIDConnectConfigurationWithClientSecret : IOpenIDConnectConfiguration
    {
        string ClientSecret { get; set; }       
    }
}