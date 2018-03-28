using System.Collections.Generic;
using System.Linq;
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
    public class OpenIDConnectWithRolesAuthTokenHandlerFixture : OpenIDFixtureBase
    {
        ILog log;
        IOpenIDConnectConfigurationWithRoleStore configurationStore;
        IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer;
        IKeyRetriever keyRetriever;
        CustomOpenIDConnectWithRoleAuthTokenHandler target;

        [SetUp]
        public void SetUp()
        {
            log = Substitute.For<ILog>();
            configurationStore = Substitute.For<IOpenIDConnectConfigurationWithRoleStore>();
            identityProviderConfigDiscoverer = Substitute.For<IIdentityProviderConfigDiscoverer>();
            keyRetriever = Substitute.For<IKeyRetriever>();

            target = new CustomOpenIDConnectWithRoleAuthTokenHandler(
                log,
                configurationStore,
                identityProviderConfigDiscoverer,
                keyRetriever);
        }

        [Test]
        public async void ShouldReturnRolesFromToken()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyPublic;
            var token = CreateToken(KeyId, out rsaSecurityKeyPublic, roleIds: new[] {"octoTesters"});
            var issuerConfig = new IssuerConfiguration() {Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyPublic } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);
            configurationStore.GetRoleClaimType().Returns("roles");

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
            Assert.IsNotNull(result.ExternalGroupIds);

            Assert.IsNotNullOrEmpty(result.ExternalGroupIds.SingleOrDefault(x => x == "octoTesters"));
        }

        [Test]
        public async void ShouldReturnGroupsFromToken()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyPublic;
            var token = CreateToken(KeyId, out rsaSecurityKeyPublic, groupIds: new[] {"octoTesters"});
            var issuerConfig = new IssuerConfiguration() {Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyPublic } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);
            configurationStore.GetRoleClaimType().Returns("groups");

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
            Assert.IsNotNull(result.ExternalGroupIds);

            Assert.IsNotNullOrEmpty(result.ExternalGroupIds.SingleOrDefault(x => x == "octoTesters"));
        }

        [Test]
        public async void ShouldReturnNoRolesWhenOnlyGroupsAreInToken()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyPublic;
            var token = CreateToken(KeyId, out rsaSecurityKeyPublic, groupIds: new[] {"octoTesters"});
            var issuerConfig = new IssuerConfiguration() {Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyPublic } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);
            configurationStore.GetRoleClaimType().Returns("roles");

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
            Assert.IsNotNull(result.ExternalGroupIds);

            Assert.IsEmpty(result.ExternalGroupIds);
        }

        [Test]
        public async void ShouldReturnRolesAndGroupsWhenBothAreInToken()
        {
            // Arrange
            RsaSecurityKey rsaSecurityKeyPublic;
            var token = CreateToken(KeyId, out rsaSecurityKeyPublic, roleIds: new[] {"octoTestersRole"}, groupIds: new[] {"octoTestersGroup"});
            var issuerConfig = new IssuerConfiguration() {Issuer = DefaultIssuer };
            var key = new Dictionary<string, AsymmetricSecurityKey>() { { KeyId, rsaSecurityKeyPublic } };

            var request = CreateRequest(token);

            configurationStore.GetIssuer().Returns(DefaultIssuer);
            configurationStore.GetClientId().Returns(DefaultClientId);
            configurationStore.GetRoleClaimType().Returns("groups");

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
            Assert.IsNotNull(result.ExternalGroupIds);

            Assert.IsNotNullOrEmpty(result.ExternalGroupIds.SingleOrDefault(x => x == "octoTestersRole"));
            Assert.IsNotNullOrEmpty(result.ExternalGroupIds.SingleOrDefault(x => x == "octoTestersGroup"));
        }
    }
}