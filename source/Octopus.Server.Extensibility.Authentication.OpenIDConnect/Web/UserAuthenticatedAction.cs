using System;
using System.Globalization;
using System.IdentityModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nancy;
using Nancy.Cookies;
using Nancy.Helpers;
using Octopus.Data.Storage.User;
using Octopus.Diagnostics;
using Octopus.Server.Extensibility.Authentication.HostServices;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;
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

            string stateFromRequest;
            var principalContainer = await authTokenHandler.GetPrincipalAsync(context.Request, out stateFromRequest);
            var principal = principalContainer.principal;
            if (principal == null || !string.IsNullOrEmpty(principalContainer.error))
            {
                var message = $"The response from the external identity provider contained an error: {principalContainer.error}";
                log.Error(message);
                return ResponseCreator.BadRequest(message);
            }

            const string stateDescription = "As a security precaution, Octopus ensures the state object returned from the external identity provider matches what it expected.";
            var expectedStateHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("s"))
                expectedStateHash = HttpUtility.UrlDecode(context.Request.Cookies["s"]);
            if (string.IsNullOrWhiteSpace(expectedStateHash))
            {
                var message = $"User login failed: Missing State Hash Cookie. {stateDescription} In this case the Cookie containing the SHA256 hash of the state object is missing from the request.";
                log.Error(message);
                return ResponseCreator.BadRequest(message);
            }

            var stateFromRequestHash = State.Protect(stateFromRequest);
            if (stateFromRequestHash != expectedStateHash)
            {
                var message = $"User login failed: Tampered State. {stateDescription} In this case the state object looks like it has been tampered with. The state object is '{stateFromRequest}'. The SHA256 hash of the state was expected to be '{expectedStateHash}' but was '{stateFromRequestHash}'.";
                log.Error(message);
                return ResponseCreator.BadRequest(message);
            }

            const string nonceDescription = "As a security precaution to prevent replay attacks, Octopus ensures the nonce returned in the claims from the external identity provider matches what it expected.";

            var expectedNonceHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("n"))
                expectedNonceHash = HttpUtility.UrlDecode(context.Request.Cookies["n"]);

            if (string.IsNullOrWhiteSpace(expectedNonceHash))
            {
                var message = $"User login failed: Missing Nonce Hash Cookie. {nonceDescription} In this case the Cookie containing the SHA256 hash of the nonce is missing from the request.";
                log.Error(message);
                return ResponseCreator.BadRequest(message);
            }

            var nonceFromClaims = principal.Claims.FirstOrDefault(c => c.Type == "nonce");
            if (nonceFromClaims == null)
            {
                var message = $"User login failed: Missing Nonce Claim. {nonceDescription} In this case the 'nonce' claim is missing from the security token.";
                log.Error(message);
                return ResponseCreator.BadRequest(message);
            }

            var nonceFromClaimsHash = Nonce.Protect(nonceFromClaims.Value);
            if (nonceFromClaimsHash != expectedNonceHash)
            {
                var message = $"User login failed: Tampered Nonce. {nonceDescription} In this case the nonce looks like it has been tampered with or reused. The nonce is '{nonceFromClaims}'. The SHA256 hash of the state was expected to be '{expectedNonceHash}' but was '{nonceFromClaimsHash}'.";
                log.Error(message);
                return ResponseCreator.BadRequest(message);
            }

            var model = principalToUserResourceMapper.MapToUserResource(principal);

            var action = loginTracker.BeforeAttempt(model.Username, context.Request.UserHostAddress);
            if (action == InvalidLoginAction.Ban)
            {
                return RedirectResponse(response, $"{stateFromRequest}?error=You have had too many failed login attempts in a short period of time. Please try again later.");
            }

            var userResult = GetOrCreateUser(model, principal);
            if (!userResult.Succeeded)
            {
                loginTracker.RecordFailure(model.Username, context.Request.UserHostAddress);

                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return RedirectResponse(response, $"{stateFromRequest}?error={userResult.FailureReason}");
            }

            if (!userResult.User.IsActive || userResult.User.IsService)
            {
                loginTracker.RecordFailure(model.Username, context.Request.UserHostAddress);

                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return RedirectResponse(response, $"{stateFromRequest}?error=Invalid username or password");
            }

            loginTracker.RecordSucess(model.Username, context.Request.UserHostAddress);

            var cookie = authCookieCreator.CreateAuthCookie(context, userResult.User.IdentificationToken, true);

            return RedirectResponse(response, stateFromRequest)
                .WithCookie(cookie)
                .WithHeader("Expires", DateTime.UtcNow.AddYears(1).ToString("R", DateTimeFormatInfo.InvariantInfo));
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