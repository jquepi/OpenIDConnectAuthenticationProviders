namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates
{
    public interface IKeyJsonParser
    {
        KeyDetails[] Parse(string content);
    }
}