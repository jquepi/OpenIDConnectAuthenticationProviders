namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Certificates
{
    public class RsaDetails : KeyDetails
    {
        public string Modulus { get; set; }
        public string Exponent { get; set; }
    }
}