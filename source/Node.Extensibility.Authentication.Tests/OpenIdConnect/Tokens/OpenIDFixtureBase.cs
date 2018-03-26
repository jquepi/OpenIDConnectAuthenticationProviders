using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using JWT;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using Nancy;
using Nancy.Helpers;
using Newtonsoft.Json;
using Node.Extensibility.Authentication.Tests.Helpers;
using NUnit.Framework;
using Octopus.Node.Extensibility.Authentication.Resources;

namespace Node.Extensibility.Authentication.Tests.OpenIdConnect.Tokens
{
    public class OpenIDFixtureBase
    {
        protected const string DefaultIssuer = "https://somedomain.com/";
        protected const string DefaultClientId = "Octopus Deploy";
        protected const string DefaultSubject = "12345";
        protected const string DefaultName = "bob";
        protected const string DefaultEmail = "bob@somedomain.com";

        protected const string KeyId = "88a8e79fba13856b4159f96e9c9ea6d5";
        protected const string Nonce = "";

        protected Request CreateRequest(string token, string redirectAfterLoginTo = "/state/", bool usingSecureConnection = false)
        {
            var request = new Request("POST", DefaultIssuer);
            request.Form["access_token"] = null;
            request.Form["id_token"] = token;
            var stateData = JsonConvert.SerializeObject(new LoginState {RedirectAfterLoginTo = redirectAfterLoginTo, UsingSecureConnection = usingSecureConnection});
            request.Form["state"] = stateData;
            return request;
        }

        protected string CreateToken(
            string keyId,
            out RsaSecurityKey key,
            string issuer = DefaultIssuer,
            string clientId = DefaultClientId,
            string nonce = Nonce,
            string subject = DefaultSubject,
            string name = DefaultName,
            string email = DefaultEmail,
            string[] roleIds = null,
            string[] groupIds = null)
        {
            var epoch = new DateTime(1970, 1, 1);
            var now = Math.Round((DateTime.Now - epoch).TotalSeconds);
            var exp = now + 36000;

            var payload = new Dictionary<string, object>
            {
                { "aud", clientId },
                { "iss", issuer },
                { "exp", exp },
                { "iat", now },
                { "nonce", nonce },
                { "sub", subject},
                { "name", name},
                { "email", email}
            };

            var headers = new Dictionary<string, object>
            {
                { "kid",  keyId}
            };

            if (roleIds != null && roleIds.Any())
            {
                payload.Add("roles", roleIds);
            }
            if (groupIds != null && groupIds.Any())
            {
                payload.Add("groups", groupIds);
            }

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                var rsaParametersPublic = rsa.ExportParameters(includePrivateParameters: false);
                key = new RsaSecurityKey(rsaParametersPublic) {KeyId = keyId};

                var algorithm = new RS256AlgorithmRsaOnly(rsa);
                var serializer = new JsonNetSerializer();
                var urlEncoder = new JwtBase64UrlEncoder();
                var encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                var token = encoder.Encode(headers, payload, string.Empty);

                return token;
            }
        }

        protected void AssertClaims(
            ClaimsPrincipal principal,
            string issuer = DefaultIssuer,
            string clientid = DefaultClientId,
            string nonce = Nonce,
            string subject = DefaultSubject,
            string name = DefaultName,
            string email = DefaultEmail)
        {
            Assert.IsTrue(principal.HasClaim(x => x.Type == "iss" && x.Value == issuer));
            Assert.IsTrue(principal.HasClaim(x => x.Type == "aud" && x.Value == clientid));
            Assert.IsTrue(principal.HasClaim(x => x.Type == "nonce" && x.Value == nonce));
            Assert.IsTrue(principal.HasClaim(x => x.Type == "name" && x.Value == name));
            Assert.IsTrue(principal.HasClaim(x => x.Type == ClaimTypes.NameIdentifier && x.Value == subject));
            Assert.IsTrue(principal.HasClaim(x => x.Type == ClaimTypes.Email && x.Value == email));
        }
    }
}