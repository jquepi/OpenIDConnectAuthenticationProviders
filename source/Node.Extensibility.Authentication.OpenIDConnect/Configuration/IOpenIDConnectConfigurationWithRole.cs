namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public interface IOpenIDConnectConfigurationWithRole : IOpenIDConnectConfiguration
    {
        string RoleClaimType { get; set; }       
    }
}