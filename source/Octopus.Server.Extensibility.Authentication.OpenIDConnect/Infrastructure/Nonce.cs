using System;
using System.Security.Cryptography;
using System.Text;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure
{
    public static class Nonce
    {
        static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();

        public static string GenerateUrlSafeNonce()
        {
            var data = new byte[16];
            Rng.GetNonZeroBytes(data);
            var nonce = Convert.ToBase64String(data).TrimEnd('=').Replace("/", string.Empty).Replace("+", string.Empty);
            return nonce;
        }

        public static string Protect(string nonce)
        {
            using (var sha = new SHA256CryptoServiceProvider())
            {
                var nonceHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("OctoNonce" + nonce)));
                return nonceHash;
            }
        }
    }
}