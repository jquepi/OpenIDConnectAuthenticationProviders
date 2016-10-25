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
        readonly IPrincipalToUserHandler principalToUserHandler;
        readonly IUserStore userStore;
        readonly IAuthCookieCreator authCookieCreator;
        readonly IInvalidLoginTracker loginTracker;
        readonly ISleep sleep;

        protected readonly TStore ConfigurationStore;

        protected UserAuthenticatedAction(
            ILog log,
            TAuthTokenHandler authTokenHandler,
            IPrincipalToUserHandler principalToUserHandler,
            IUserStore userStore,
            TStore configurationStore,
            IAuthCookieCreator authCookieCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep)
        {
            this.log = log;
            this.authTokenHandler = authTokenHandler;
            this.principalToUserHandler = principalToUserHandler;
            this.userStore = userStore;
            ConfigurationStore = configurationStore;
            this.authCookieCreator = authCookieCreator;
            this.loginTracker = loginTracker;
            this.sleep = sleep;
        }

        public async Task<Response> ExecuteAsync(NancyContext context, IResponseFormatter response)
        {
            string state;
            var principalContainer = await authTokenHandler.GetPrincipalAsync(context.Request, out state);
            var principal = principalContainer.principal;
            if (principal == null || !string.IsNullOrEmpty(principalContainer.error))
            {
                log.Info($"User login failed - {principalContainer.error}");
                return RedirectResponse(response, $"{context.Request.Form["state"]}?error={principalContainer.error}");
            }

            var cookieStateHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("s"))
                cookieStateHash = HttpUtility.UrlDecode(context.Request.Cookies["s"]);
            if (State.Protect(state) != cookieStateHash)
            {
                log.Info($"User login failed - invalid state");
                return RedirectResponse(response, $"{state}?error=Invalid state");
            }

            var cookieNonceHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("n"))
                cookieNonceHash = HttpUtility.UrlDecode(context.Request.Cookies["n"]);
            
            var nonce = principal.Claims.FirstOrDefault(c => c.Type == "nonce");
            if (nonce == null || Nonce.Protect(nonce.Value) != cookieNonceHash)
            {
                log.Info("User login failed - invalid nonce");
                return RedirectResponse(response, $"{state}?error=Invalid nonce");
            }

            var model = principalToUserHandler.GetUserResource(principal);

            var action = loginTracker.BeforeAttempt(model.Username, context.Request.UserHostAddress);
            if (action == InvalidLoginAction.Ban)
            {
                return RedirectResponse(response, $"{state}?error=You have had too many failed login attempts in a short period of time. Please try again later.");
            }

            var userResult = GetOrCreateUser(model, principal);
            if (!userResult.Succeeded)
            {
                loginTracker.RecordFailure(model.Username, context.Request.UserHostAddress);

                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return RedirectResponse(response, $"${state}?error={userResult.FailureReason}");
            }

            if (!userResult.User.IsActive || userResult.User.IsService)
            {
                loginTracker.RecordFailure(model.Username, context.Request.UserHostAddress);

                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return RedirectResponse(response, $"{state}?error=Invalid username or password");
            }

            loginTracker.RecordSucess(model.Username, context.Request.UserHostAddress);

            var cookie = authCookieCreator.CreateAuthCookie(context, userResult.User.IdentificationToken, true);

            return RedirectResponse(response, state)
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