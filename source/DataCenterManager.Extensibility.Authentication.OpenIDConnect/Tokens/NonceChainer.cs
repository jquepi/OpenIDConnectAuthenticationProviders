using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens
{
    public class NonceChainer : INonceChainer
    {
        public string Chain(string callerNonce, string chainedNonce)
        {
            return $"{callerNonce}.{chainedNonce}";
        }

        public string Delink(string returnedNonce)
        {
            return !returnedNonce.Contains(".") ? returnedNonce : returnedNonce.Split('.')[0];
        }
    }

    public interface INonceChainer
    {
        string Chain(string callerNonce, string chainedNonce);
        string Delink(string returnedNonce);
    }
}