using System;
using System.Security.Cryptography;
using System.Text;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure
{
    public static class Nonce
    {
        public static string Generate()
        {
            var nonce = Guid.NewGuid().ToString("N");
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