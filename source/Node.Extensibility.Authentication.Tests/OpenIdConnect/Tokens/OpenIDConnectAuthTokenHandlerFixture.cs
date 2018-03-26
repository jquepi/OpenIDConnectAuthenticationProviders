using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Nancy;
using NSubstitute;
using NUnit.Framework;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.Resources;

namespace Node.Extensibility.Authentication.Tests.OpenIdConnect.Tokens
{
    [TestFixture]
    public class OpenIDConnectAuthTokenHandlerFixture : OpenIDFixtureBase
    {
        ILog log;
        IOpenIDConnectConfigurationStore configurationStore;
        IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        IKeyRetriever keyRetriever;
        CustomOpenIDConnectAuthTokenHandler target;

        [SetUp]
        public void SetUp()
        {
            log = Substitute.For<ILog>();
            configurationStore = Substitute.For<IOpenIDConnectConfigurationStore>();
            identityProviderConfigDiscoverer = Substitute.For<IIdentityProviderConfigDiscoverer>();
            keyRetriever = Substitute.For<IKeyRetriever>();

            target = new CustomOpenIDConnectAuthTokenHandler(
                log,
                configurationStore,
                identityProviderConfigDiscoverer,
                keyRetriever);
        }

        [Test]
        public async void ShouldReturnPrincipalWhenValidTokenReceived()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyPublic;
            var token = CreateToken(KeyId, out rsaSecurityKeyPublic);
            var issuerConfig = new IssuerConfiguration() {Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyPublic } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);

            identityProviderConfigDiscoverer.GetConfigurationAsync(DefaultIssuer)
                .Returns(Task.FromResult<IssuerConfiguration>(issuerConfig));

            keyRetriever.GetKeysAsync(issuerConfig, false)
                .Returns(Task.FromResult<IDictionary<string, AsymmetricSecurityKey>>(key));

            // Act
            LoginState state;
            var result = await target.GetPrincipalAsync(((DynamicDictionary)request.Form).ToDictionary(), out state);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNullOrEmpty(result.Error);
            Assert.IsNotNull(result.Principal);
            Assert.AreEqual("/state/", state.RedirectAfterLoginTo);

            AssertClaims(result.Principal);

            configurationStore.Received().GetIssuer();
            configurationStore.Received().GetClientId();
            Received.InOrder(async () => { await keyRetriever.GetKeysAsync(issuerConfig, false); });

        }


        [Test]
        public async void ShouldRetryWhenInvalidKeyDetected()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyStale;
            RsaSecurityKey rsaSecurityKeyCurrent;

            var token = CreateToken("99a8e79fba13856b4159f96e9c9ea6d5", out rsaSecurityKeyStale);
            token = CreateToken(KeyId, out rsaSecurityKeyCurrent);
            var issuerConfig = new IssuerConfiguration() { Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyCurrent } };
            var staleKey = new Dictionary<string, AsymmetricSecurityKey>() { { "99a8e79fba13856b4159f96e9c9ea6d5", rsaSecurityKeyStale } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);

            identityProviderConfigDiscoverer.GetConfigurationAsync(DefaultIssuer)
                .Returns(Task.FromResult<IssuerConfiguration>(issuerConfig));

            // On first call to keyRetriever return invalid (stale) key
            keyRetriever.GetKeysAsync(issuerConfig, false)
                .Returns(Task.FromResult<IDictionary<string, AsymmetricSecurityKey>>(staleKey));

            // On second call return the new valid key
            keyRetriever.GetKeysAsync(issuerConfig, true)
                .Returns(Task.FromResult<IDictionary<string, AsymmetricSecurityKey>>(key));

            // Act
            LoginState state;
            var result = await target.GetPrincipalAsync(((DynamicDictionary)request.Form).ToDictionary(), out state);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNullOrEmpty(result.Error);
            Assert.IsNotNull(result.Principal);
            Assert.AreEqual("/state/", state.RedirectAfterLoginTo);

            AssertClaims(result.Principal);

            configurationStore.Received().GetIssuer();
            configurationStore.Received().GetClientId();

            Received.InOrder(async () => { await keyRetriever.GetKeysAsync(issuerConfig, false); });
            Received.InOrder(async () => { await keyRetriever.GetKeysAsync(issuerConfig, true); });
        }


        [Test]
        [ExpectedException(typeof(SecurityTokenInvalidSignatureException))]
        public async void ShouldThrowWhenInvalidKeyDetectedTwice()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyStale;
            RsaSecurityKey rsaSecurityKeyCurrent;

            var token = CreateToken("99a8e79fba13856b4159f96e9c9ea6d5", out rsaSecurityKeyStale);
            token = CreateToken(KeyId, out rsaSecurityKeyCurrent);
            var issuerConfig = new IssuerConfiguration() { Issuer = DefaultIssuer };
            var staleKey = new Dictionary<string, AsymmetricSecurityKey>() { { "99a8e79fba13856b4159f96e9c9ea6d5", rsaSecurityKeyStale } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);

            identityProviderConfigDiscoverer.GetConfigurationAsync(DefaultIssuer)
                .Returns(Task.FromResult<IssuerConfiguration>(issuerConfig));

            // set keyRetriever to always return invalid (stale) key
            keyRetriever.GetKeysAsync(issuerConfig)
                .ReturnsForAnyArgs(Task.FromResult<IDictionary<string, AsymmetricSecurityKey>>(staleKey));

            // Act
            LoginState state;
            var result = await target.GetPrincipalAsync(((DynamicDictionary)request.Form).ToDictionary(), out state);

            // Expect Exception Thrown

        }

        [Test]
        [TestCase("https://someotherdomain.com", DefaultClientId, Description = "Invalid Issuer", ExpectedException = typeof(SecurityTokenInvalidIssuerException))]
        [TestCase(DefaultIssuer, "Another Client", Description = "Invalid ClientId", ExpectedException = typeof(SecurityTokenInvalidAudienceException))]
        public async void ShouldThrowWhenInvalidTokenDetected(string issuer, string clientId)
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyPublic;
            var token = CreateToken(KeyId, out rsaSecurityKeyPublic, issuer: issuer, clientId: clientId);
            var issuerConfig = new IssuerConfiguration() { Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyPublic } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);

            identityProviderConfigDiscoverer.GetConfigurationAsync(DefaultIssuer)
                .Returns(Task.FromResult<IssuerConfiguration>(issuerConfig));

            keyRetriever.GetKeysAsync(issuerConfig, false)
                .Returns(Task.FromResult<IDictionary<string, AsymmetricSecurityKey>>(key));

            // Act
            LoginState state;
            var result = await target.GetPrincipalAsync(((DynamicDictionary)request.Form).ToDictionary(), out state);

            // Expect Exception Thrown

        }
    }
}
