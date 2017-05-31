namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect.Infrastructure
{
    public class UserResource
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string ExternalId { get; set; }
    }
}