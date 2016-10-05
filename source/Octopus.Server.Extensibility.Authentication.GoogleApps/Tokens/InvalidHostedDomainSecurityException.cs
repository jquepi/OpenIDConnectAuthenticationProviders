using System.Security;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens
{
    public class InvalidHostedDomainSecurityException : SecurityException
    {
        public InvalidHostedDomainSecurityException() : base("Incorrect Hosted Domain value")
        {
        }
    }
}