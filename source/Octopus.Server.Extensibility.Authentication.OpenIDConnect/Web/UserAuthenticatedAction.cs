using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nancy;
using Nancy.Cookies;
using Nancy.Helpers;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Api;
using Octopus.Server.Extensibility.HostServices.Authentication;
using Octopus.Server.Extensibility.HostServices.Model;
using Octopus.Server.Extensibility.HostServices.Time;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Web
{
    public abstract class UserAuthenticatedAction<TStore, TAuthTokenHandler> : IAsyncApiAction
        where TStore : IOpenIDConnectConfigurationStore
        where TAuthTokenHandler : IAuthTokenHandler
    {
        readonly TAuthTokenHandler authTokenHandler;
        readonly IPrincipalToUserHandler principalToUserHandler;
        readonly IUserStore userStore;
        readonly IAuthCookieCreator authCookieCreator;
        readonly IApiActionResponseCreator responseCreator;
        readonly IInvalidLoginTracker loginTracker;
        readonly ISleep sleep;

        protected readonly TStore ConfigurationStore;

        protected UserAuthenticatedAction(
            TAuthTokenHandler authTokenHandler,
            IPrincipalToUserHandler principalToUserHandler,
            IUserStore userStore,
            TStore configurationStore,
            IAuthCookieCreator authCookieCreator,
            IApiActionResponseCreator responseCreator,
            IInvalidLoginTracker loginTracker,
            ISleep sleep)
        {
            this.authTokenHandler = authTokenHandler;
            this.principalToUserHandler = principalToUserHandler;
            this.userStore = userStore;
            ConfigurationStore = configurationStore;
            this.authCookieCreator = authCookieCreator;
            this.responseCreator = responseCreator;
            this.loginTracker = loginTracker;
            this.sleep = sleep;
        }

        public async Task<Response> ExecuteAsync(NancyContext context, IResponseFormatter response)
        {
            string state;
            var principal = await authTokenHandler.GetPrincipalAsync(context.Request, out state);

            if (principal == null)
            {
                return new Response { StatusCode = HttpStatusCode.Unauthorized };
            }

            var cookieStateHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("s"))
                cookieStateHash = HttpUtility.UrlDecode(context.Request.Cookies["s"]);
            if (State.Protect(state) != cookieStateHash)
            {
                return new Response { StatusCode = HttpStatusCode.Unauthorized };
            }

            var cookieNonceHash = string.Empty;
            if (context.Request.Cookies.ContainsKey("n"))
                cookieNonceHash = HttpUtility.UrlDecode(context.Request.Cookies["n"]);
            
            var nonce = principal.Claims.FirstOrDefault(c => c.Type == "nonce");
            if (nonce == null || Nonce.Protect(nonce.Value) != cookieNonceHash)
            {
                return new Response { StatusCode = HttpStatusCode.Unauthorized };
            }

            var model = principalToUserHandler.GetUserResource(principal);

            var action = loginTracker.BeforeAttempt(model.Username, context.Request.UserHostAddress);
            if (action == InvalidLoginAction.Ban)
            {
                return responseCreator.BadRequest("You have had too many failed login attempts in a short period of time. Please try again later.");
            }

            var userResult = GetOrCreateUser(model, principal);
            if (!userResult.Succeeded)
            {
                loginTracker.RecordFailure(model.Username, context.Request.UserHostAddress);

                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return responseCreator.BadRequest(HttpStatusCode.BadRequest, userResult.FailureReason);
            }

            if (!userResult.User.IsActive || userResult.User.IsService)
            {
                loginTracker.RecordFailure(model.Username, context.Request.UserHostAddress);

                if (action == InvalidLoginAction.Slow)
                {
                    sleep.For(1000);
                }

                return responseCreator.BadRequest("Invalid username or password.");
            }

            loginTracker.RecordSucess(model.Username, context.Request.UserHostAddress);

            var cookie = authCookieCreator.CreateAuthCookie(context, userResult.User.IdentificationToken, true);

            return response.AsRedirect(state)
                .WithCookie(cookie)
                .WithCookie(new NancyCookie("s", Guid.NewGuid().ToString(), true, false, DateTime.MinValue))
                .WithCookie(new NancyCookie("n", Guid.NewGuid().ToString(), true, false, DateTime.MinValue))
                .WithHeader("Expires", DateTime.UtcNow.AddYears(1).ToString("R", DateTimeFormatInfo.InvariantInfo));

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