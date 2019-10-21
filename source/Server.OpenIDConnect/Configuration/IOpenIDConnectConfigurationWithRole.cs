namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public interface IOpenIDConnectConfigurationWithRole : IOpenIDConnectConfiguration
    {
        string RoleClaimType { get; set; }       
    }
}