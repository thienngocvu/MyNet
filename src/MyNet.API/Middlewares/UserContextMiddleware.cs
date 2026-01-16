using MyNet.Application.Common;
using MyNet.Application.Common.Cache;
using MyNet.Application.Common.Extensions;
using MyNet.Application.DTOs.Response;
using MyNet.Application.Services;
using System.Security.Claims;
using static MyNet.Application.Common.Constants.AppConstant;

namespace MyNet.API.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICache _cache;
        private readonly ILogger<UserContextMiddleware> _logger;

        public UserContextMiddleware(
            RequestDelegate next
            , IConfiguration configuration
            , ICache cache
            , ILogger<UserContextMiddleware> logger
        )
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity!.IsAuthenticated)
            {
                await HandleUnauthorized(context);
                return;
            }
            else
            {
                // For JWT, we use ClaimTypes.NameIdentifier or ClaimTypes.Name
                var uidClaim = context.User.Claims.FirstOrDefault(x => 
                    x.Type == ClaimTypes.NameIdentifier || 
                    x.Type == ClaimTypes.Name);

                if (uidClaim != null)
                {
                    var continueProcessing = await HandleAuthenticatedUser(context, uidClaim);
                    if (!continueProcessing)
                    {
                        return;
                    }
                }
                else
                {
                    await HandleUnauthorized(context);
                    return;
                }
            }

            await _next(context);
        }

        private async Task HandleUnauthorized(HttpContext context)
        {
            context.Response.StatusCode = StatusCodeResponse.UNAUTHORIZED;
            await context.Response.WriteAsync("Unauthorized");
        }

        private async Task<bool> HandleAuthenticatedUser(HttpContext context, Claim uidClaim)
        {
            var userId = uidClaim.Value;
            var hash = userId.StringToMd5();
            var keyHashed = $"user_id_{hash}";

            var userInfo = await _cache.GetAsync<UserDto>(keyHashed);
            var userContext = context.RequestServices.GetService(typeof(IUserContext)) as IUserContext;

            if (userInfo == null) 
            {
                var userService = context.RequestServices.GetRequiredService<IUserService>();
                userInfo = await userService.FindlByUserIdAsync(userId);

                if (userInfo == null)
                {
                    context.Response.StatusCode = StatusCodeResponse.UNAUTHORIZED;
                    await context.Response.WriteAsync("Unauthorized");
                    return false;
                }
                else
                {
                    await _cache.StoreAsync(keyHashed, userInfo);
                }
            }

            if (userInfo != null)
            {
                userContext?.SetId(userInfo.UserId ?? string.Empty);
                userContext?.SetName(userInfo.Name ?? string.Empty);
                userContext?.SetRoleName(AuthzPolicy.USER_ROLE);
                userContext?.SetRights(userInfo.Roles);
            }

            return true;
        }
    }
}
