namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration
{
    public interface IOpenIDConnectConfigurationWithClientSecret : IOpenIDConnectConfiguration
    {
        string ClientSecret { get; set; }       
    }
}