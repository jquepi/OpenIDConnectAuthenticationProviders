using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octopus.Data.Model.User;
using Octopus.DataCenterManager.Extensibility.Authentication.HostServices;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web.Models;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Identities;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.Resources.Identities;
using Octopus.Node.Extensibility.Authentication.Storage.User;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Time;

namespace Octopus.DataCenterManager.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class AuthenticatedController<TStore, TAuthTokenHandler, TIdentityCreator> : Controller
        where TStore : IOpenIDConnectConfigurationStore
        where TAuthTokenHandler : IAuthTokenHandler
        where TIdentityCreator : IIdentityCreator
    {
        readonly ILog log;
        readonly TAuthTokenHandler authTokenHandler;
        readonly IPrincipalToUserResourceMapper principalToUserResourceMapper;
        readonly IUpdateableUserStore userStore;
        readonly TStore configurationStore;
        readonly IInvalidLoginTracker loginTracker;
        readonly IUrlEncoder urlEncoder;
        readonly ISleep sleep;
        readonly IClock clock;
        readonly IAuthCookieCreator authCookieCreator;
        readonly IJwtCreator jwtCreator;
        readonly INonceChainer nonceChainer;
        readonly IStateChainer stateChainer;
        readonly TIdentityCreator identityCreator;

        protected AuthenticatedController(
            ILog log,
            TAuthTokenHandler authTokenHandler,
            IPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUpdateableUserStore userStore,
            TStore configurationStore,
            IInvalidLoginTracker loginTracker,
            IUrlEncoder urlEncoder,
            ISleep sleep,
            IClock clock,
            IAuthCookieCreator authCookieCreator,
            IJwtCreator jwtCreator,
            INonceChainer nonceChainer,
            IStateChainer stateChainer,
            TIdentityCreator identityCreator)
        {
            this.log = log;
            this.authTokenHandler = authTokenHandler;
            this.principalToUserResourceMapper = principalToUserResourceMapper;
            this.userStore = userStore;
            this.configurationStore = configurationStore;
            this.loginTracker = loginTracker;
            this.urlEncoder = urlEncoder;
            this.sleep = sleep;
            this.clock = clock;
            this.authCookieCreator = authCookieCreator;
            this.jwtCreator = jwtCreator;
            this.nonceChainer = nonceChainer;
            this.stateChainer = stateChainer;
            this.identityCreator = identityCreator;
        }
        
        protected abstract string ProviderName { get; }
        
        protected async Task<IActionResult> ProcessAuthenticated()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                string stateFromRequest;
                var principalContainer = await authTokenHandler.GetPrincipalAsync(Request.Form, out stateFromRequest);
                var principal = principalContainer.principal;
                if (principal == null || !string.IsNullOrEmpty(principalContainer.error))
                {
                    return BadRequest(
                        $"The response from the external identity provider contained an error: {principalContainer.error}");
                }

                // Step 2: Validate the state object we passed wasn't tampered with
                const string stateDescription =
                    "As a security precaution, Octopus ensures the state object returned from the external identity provider matches what it expected.";
                var expectedStateHash = string.Empty;
                if (Request.Cookies.ContainsKey(UserAuthConstants.DCMStateCookieName))
                    expectedStateHash = urlEncoder.UrlDecode(Request.Cookies[UserAuthConstants.DCMStateCookieName]);
                if (string.IsNullOrWhiteSpace(expectedStateHash))
                {
                    return BadRequest(
                        $"User login failed: Missing State Hash Cookie. {stateDescription} In this case the Cookie containing the SHA256 hash of the state object is missing from the request.");
                }

                var stateFromRequestHash = State.Protect(stateFromRequest);
                if (stateFromRequestHash != expectedStateHash)
                {
                    log.ErrorFormat("Tampered state. stateFromRequest: {0} => {1} expectedStateHash: {2}",
                        stateFromRequest, stateFromRequestHash, expectedStateHash);
                    return BadRequest(
                        $"User login failed: Tampered State. {stateDescription} In this case the state object looks like it has been tampered with. The state object is '{stateFromRequest}'. The SHA256 hash of the state was expected to be '{expectedStateHash}' but was '{stateFromRequestHash}'.");
                }

                // Step 3: Validate the nonce is as we expected to prevent replay attacks
                const string nonceDescription =
                    "As a security precaution to prevent replay attacks, Octopus ensures the nonce returned in the claims from the external identity provider matches what it expected.";

                var expectedNonceHash = string.Empty;
                if (Request.Cookies.ContainsKey(UserAuthConstants.DCMNonceCookieName))
                    expectedNonceHash = urlEncoder.UrlDecode(Request.Cookies[UserAuthConstants.DCMNonceCookieName]);

                if (string.IsNullOrWhiteSpace(expectedNonceHash))
                {
                    return BadRequest(
                        $"User login failed: Missing Nonce Hash Cookie. {nonceDescription} In this case the Cookie containing the SHA256 hash of the nonce is missing from the request.");
                }

                var nonceFromClaims = principal.Claims.FirstOrDefault(c => c.Type == "nonce");
                if (nonceFromClaims == null)
                {
                    return BadRequest(
                        $"User login failed: Missing Nonce Claim. {nonceDescription} In this case the 'nonce' claim is missing from the security token.");
                }

                var nonceFromClaimsHash = Nonce.Protect(nonceFromClaims.Value);
                if (nonceFromClaimsHash != expectedNonceHash)
                {
                    log.ErrorFormat("Tampered nonce. nonceFromClaims: {0} => {1} expectedNonceHash: {2}",
                        nonceFromClaims, nonceFromClaimsHash, expectedNonceHash);
                    return BadRequest(
                        $"User login failed: Tampered Nonce. {nonceDescription} In this case the nonce looks like it has been tampered with or reused. The nonce is '{nonceFromClaims}'. The SHA256 hash of the state was expected to be '{expectedNonceHash}' but was '{nonceFromClaimsHash}'.");
                }

                // Step 4: Now the integrity of the request has been validated we can figure out which Octopus User this represents
                var authenticationCandidate = principalToUserResourceMapper.MapToUserResource(principal);

                // Step 4a: Check if this authentication attempt is already being banned
                var action = loginTracker.BeforeAttempt(authenticationCandidate.Username,
                    Request.HttpContext.Connection.RemoteIpAddress.ToString());
                if (action == InvalidLoginAction.Ban)
                {
                    return BadRequest(
                        "You have had too many failed login attempts in a short period of time. Please try again later.");
                }

                // Step 4b: Try to get or create a the Octopus User this external identity represents
                var userResult = GetOrCreateUser(authenticationCandidate, cts.Token);
                if (userResult.Succeeded)
                {
                    if (!userResult.User.IsActive)
                    {
                        return BadRequest(
                            $"The Octopus User Account '{authenticationCandidate.Username}' has been disabled by an Administrator. If you believe this to be a mistake, please contact your Octopus Administrator to have your account re-enabled.");
                    }

                    if (userResult.User.IsService)
                    {
                        return BadRequest(
                            $"The Octopus User Account '{authenticationCandidate.Username}' is a Service Account, which are prevented from using Octopus interactively. Service Accounts are designed to authorize external systems to access the Octopus API using an API Key.");
                    }

                    var groups = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
                    if (groups.Any())
                    {
                        userStore.SetSecurityGroupIds(ProviderName, userResult.User.Id, groups, clock.GetUtcTime());
                    }

                    loginTracker.RecordSucess(authenticationCandidate.Username,
                        Request.HttpContext.Connection.RemoteIpAddress.ToString());

                    var states = stateChainer.Delink(stateFromRequest);
                    var redirectAfterLoginTo = states[0];

                    // Invalidate the state and nonce cookies
                    Response.Cookies.Append(UserAuthConstants.DCMStateCookieName, Guid.NewGuid().ToString(),
                        new CookieOptions {Secure = false, HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(20)});
                    Response.Cookies.Append(UserAuthConstants.DCMNonceCookieName, Guid.NewGuid().ToString(),
                        new CookieOptions {Secure = false, HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(20)});

                    // if there is no chained state, then the auth call originated from the DCM UI
                    if (states.Length == 1)
                    {
                        authCookieCreator.CreateAuthCookies(Response, userResult.User.IdentificationToken,
                            SessionExpiry.TwentyDays, Request.IsHttps);

                        return Redirect(redirectAfterLoginTo);
                    }

                    // otherwise the call came from a Space, so we need to return a JWT. 2nd state from the chain will be the clientId.
                    // 3rd is the redirectUrl for where to pass the token back to.
                    var clientId = states[1];
                    var redirectUrl = states[2];

                    var token = jwtCreator.CreateFor(userResult.User, nonceChainer.Delink(nonceFromClaims.Value),
                        clientId);
                    Response.Headers["Cache-Control"] = "no-cache, no-store";
                    Response.Headers["Pragma"] = "no-cache";

                    return View("~/Views/JwtToken.cshtml",
                        new JwtTokenViewModel(redirectUrl, redirectAfterLoginTo, token));
                }

                // Step 5: Handle other types of failures
                loginTracker.RecordFailure(authenticationCandidate.Username,
                    Request.HttpContext.Connection.RemoteIpAddress.ToString());

                // Step 5a: Slow this potential attacker down a bit since they seem to keep failing
                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return BadRequest($"User login failed: {userResult.FailureReason}");
            }
        }

        UserCreateResult GetOrCreateUser(UserResource userResource, CancellationToken cancellationToken)
        {
            var identityToMatch = NewIdentity(userResource);

            var user = userStore.GetByIdentity(identityToMatch);

            if (user != null)
            {
                var identity = user.Identities.FirstOrDefault(x => MatchesProviderAndExternalId(userResource, x));
                if (identity != null)
                {
                    return new UserCreateResult(user);
                }

                identity = user.Identities.FirstOrDefault(x => x.IdentityProviderName == ProviderName && x.Claims[ClaimDescriptor.EmailClaimType].Value == userResource.EmailAddress);
                if (identity != null)
                {
                    return new UserCreateResult(userStore.UpdateIdentity(user.Id, identityToMatch, cancellationToken));
                }

                return new UserCreateResult(userStore.AddIdentity(user.Id, identityToMatch, cancellationToken));
            }

            if (!configurationStore.GetAllowAutoUserCreation())
                return new AuthenticationUserCreateResult("User could not be located and auto user creation is not enabled.");

            var userResult = userStore.Create(
                userResource.Username,
                userResource.DisplayName,
                userResource.EmailAddress,
                cancellationToken,
                identities: new[] { identityToMatch });

            return userResult;
        }

        bool MatchesProviderAndExternalId(UserResource userResource, Identity x)
        {
            return x.IdentityProviderName == ProviderName && x.Claims.ContainsKey(IdentityCreator.ExternalIdClaimType) && x.Claims[IdentityCreator.ExternalIdClaimType].Value == userResource.ExternalId;
        }

        Identity NewIdentity(UserResource userResource)
        {
            return identityCreator.Create(userResource.EmailAddress, userResource.DisplayName, userResource.ExternalId);
        }
    }
}