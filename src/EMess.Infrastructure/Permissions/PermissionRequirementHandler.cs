using EMess.Application.Identity.Users;
using EMess.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace EMess.Infrastructure.Permissions
{
    internal class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;

        public PermissionRequirementHandler(IUserService userService) =>
            _userService = userService;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User?.GetUserId() is { } userId &&
                await _userService.HasPermissionAsync(userId, requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
