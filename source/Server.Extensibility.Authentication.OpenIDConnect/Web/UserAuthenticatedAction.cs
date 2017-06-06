using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nancy;
using Nancy.Cookies;
using Nancy.Helpers;
using Octopus.Data.Storage.User;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.HostServices;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Configuration;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Time;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class UserAuthenticatedAction<TStore, TAuthTokenHandler> : IAsyncApiAction
        where TStore : IOpenIDConnectConfigurationStore
        where TAuthTokenHandler : IAuthTokenHandler
    {
        readonly ILog log;
        readonly TAuthTokenHandler authTokenHandler;
        readonly IPrincipalToUserResourceMapper principalToUserResourceMapper;
        readonly IUserStore userStore;
        readonly IAuthCookieCreator authCookieCreator;
        readonly IInvalidLoginTracker loginTracker;
        readonly ISleep sleep;

        protected readonly TStore ConfigurationStore;
        protected readonly IApiActionResponseCreator ResponseCreator;

        protected UserAuthenticatedAction(
            ILog log,
            TAuthTokenHandler authTokenHandler,
            IPrincipalToUserResourceMapper principalToUserResourceMapper,
            IUserStore userStore,
            TStore configurationStore,
            IApiActionResponseCreator responseCreator, 
            IAuthCookieCreator authCookieCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep)
        {
            this.log = log;
            this.authTokenHandler = authTokenHandler;
            this.principalToUserResourceMapper = principalToUserResourceMapper;
            this.userStore = userStore;
            ConfigurationStore = configurationStore;
            ResponseCreator = responseCreator;
            this.authCookieCreator = authCookieCreator;
            this.loginTracker = loginTracker;
            this.sleep = sleep;
        }

        public async Task<Response> ExecuteAsync(NancyContext context, IResponseFormatter response)
        {
            // Step 1: Try and get all of the details from the request making sure there are no errors passed back from the external identity provider
            string stateFromRequest;
            var principalContainer = await authTokenHandler.GetPrincipalAsync(context.Request.Form, out stateFromRequest);
            var principal = principalContainer.principal;
            if (principal == null || !string.IsNullOrEmpty(principalContainer.error))
            {
                return BadRequest($"The response from the external identity provider contained an error: {principalContainer.error}");
            }

            // Step 2: Validate the state object we passed wasn't tampered with
            const string stateDescription = "As a security precaution, Octopus ensures the state object returned from the external identity provider matches what it expected.";
            var expectedStateHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("s"))
                expectedStateHash = HttpUtility.UrlDecode(context.Request.Cookies["s"]);
            if (string.IsNullOrWhiteSpace(expectedStateHash))
            {
                return BadRequest($"User login failed: Missing State Hash Cookie. {stateDescription} In this case the Cookie containing the SHA256 hash of the state object is missing from the request.");
            }

            var stateFromRequestHash = State.Protect(stateFromRequest);
            if (stateFromRequestHash != expectedStateHash)
            {
                return BadRequest($"User login failed: Tampered State. {stateDescription} In this case the state object looks like it has been tampered with. The state object is '{stateFromRequest}'. The SHA256 hash of the state was expected to be '{expectedStateHash}' but was '{stateFromRequestHash}'.");
            }

            // Step 3: Validate the nonce is as we expected to prevent replay attacks
            const string nonceDescription = "As a security precaution to prevent replay attacks, Octopus ensures the nonce returned in the claims from the external identity provider matches what it expected.";

            var expectedNonceHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("n"))
                expectedNonceHash = HttpUtility.UrlDecode(context.Request.Cookies["n"]);

            if (string.IsNullOrWhiteSpace(expectedNonceHash))
            {
                return BadRequest($"User login failed: Missing Nonce Hash Cookie. {nonceDescription} In this case the Cookie containing the SHA256 hash of the nonce is missing from the request.");
            }

            var nonceFromClaims = principal.Claims.FirstOrDefault(c => c.Type == "nonce");
            if (nonceFromClaims == null)
            {
                return BadRequest($"User login failed: Missing Nonce Claim. {nonceDescription} In this case the 'nonce' claim is missing from the security token.");
            }

            var nonceFromClaimsHash = Nonce.Protect(nonceFromClaims.Value);
            if (nonceFromClaimsHash != expectedNonceHash)
            {
                return BadRequest($"User login failed: Tampered Nonce. {nonceDescription} In this case the nonce looks like it has been tampered with or reused. The nonce is '{nonceFromClaims}'. The SHA256 hash of the state was expected to be '{expectedNonceHash}' but was '{nonceFromClaimsHash}'.");
            }

            // Step 4: Now the integrity of the request has been validated we can figure out which Octopus User this represents
            var authenticationCandidate = principalToUserResourceMapper.MapToUserResource(principal);

            // Step 4a: Check if this authentication attempt is already being banned
            var action = loginTracker.BeforeAttempt(authenticationCandidate.Username, context.Request.UserHostAddress);
            if (action == InvalidLoginAction.Ban)
            {
                return BadRequest("You have had too many failed login attempts in a short period of time. Please try again later.");
            }

            // Step 4b: Try to get or create a the Octopus User this external identity represents
            var userResult = GetOrCreateUser(authenticationCandidate, principal);
            if (userResult.Succeeded)
            {
                loginTracker.RecordSucess(authenticationCandidate.Username, context.Request.UserHostAddress);

                var authCookies = authCookieCreator.CreateAuthCookies(context.Request, userResult.User.IdentificationToken, SessionExpiry.TwentyDays);

                return RedirectResponse(response, stateFromRequest)
                    .WithCookies(authCookies)
                    .WithHeader("Expires", DateTime.UtcNow.AddYears(1).ToString("R", DateTimeFormatInfo.InvariantInfo));
            }

            // Step 5: Handle other types of failures
            loginTracker.RecordFailure(authenticationCandidate.Username, context.Request.UserHostAddress);

            // Step 5a: Slow this potential attacker down a bit since they seem to keep failing
            if (action == InvalidLoginAction.Slow)
            {
                sleep.For(1000);
            }

            if (!userResult.User.IsActive)
            {
                return BadRequest($"The Octopus User Account '{authenticationCandidate.Username}' has been disabled by an Administrator. If you believe this to be a mistake, please contact your Octopus Administrator to have your account re-enabled.");
            }

            if (userResult.User.IsService)
            {
                return BadRequest($"The Octopus User Account '{authenticationCandidate.Username}' is a Service Account, which are prevented from using Octopus interactively. Service Accounts are designed to authorize external systems to access the Octopus API using an API Key.");
            }

            return BadRequest($"User login failed: {userResult.FailureReason}");
        }

        Response BadRequest(string message)
        {
            log.Error(message);
            return ResponseCreator.BadRequest(message);
        }

        Response RedirectResponse(IResponseFormatter response, string uri)
        {
            return response.AsRedirect(uri)
                .WithCookie(new NancyCookie("s", Guid.NewGuid().ToString(), true, false, DateTime.MinValue))
                .WithCookie(new NancyCookie("n", Guid.NewGuid().ToString(), true, false, DateTime.MinValue));
        }

        UserCreateOrUpdateResult GetOrCreateUser(UserResource userResource, ClaimsPrincipal principal)
        {
            var groups = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            var userResult = userStore.CreateOrUpdate(
                userResource.Username, 
                userResource.DisplayName, 
                userResource.EmailAddress,
                userResource.ExternalId,
                null,
                true,
                null,
                false,
                groups);

            return userResult;
        }
    }
}