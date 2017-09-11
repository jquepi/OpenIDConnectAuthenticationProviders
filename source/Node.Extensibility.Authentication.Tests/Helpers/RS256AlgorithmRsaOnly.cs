﻿using System.Security.Cryptography;
using JWT;

namespace Node.Extensibility.Authentication.Tests.Helpers
{
    public class RS256AlgorithmRsaOnly : IJwtAlgorithm
    {
        readonly RSACryptoServiceProvider rsa;
        public RS256AlgorithmRsaOnly(RSACryptoServiceProvider rsa)
        {
            this.rsa = rsa;
        }

        public string Name => JwtHashAlgorithm.RS256.ToString();

        public byte[] Sign(byte[] key, byte[] bytesToSign)
        {
            return rsa.SignData(bytesToSign, CryptoConfig.MapNameToOID("SHA256"));
        }
    }
}