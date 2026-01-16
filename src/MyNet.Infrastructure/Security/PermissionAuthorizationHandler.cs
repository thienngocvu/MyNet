using Microsoft.AspNetCore.Authorization;
using MyNet.Application.Interfaces;
using System.Security.Claims;

namespace MyNet.Infrastructure.Security
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizationHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return;
            }

            // Get permissions
            var permissions = await _permissionService.GetPermissionsAsync(userId);
            var requiredPermission = $"{requirement.Function}.{requirement.Action}";

            if (permissions.Contains(requiredPermission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
