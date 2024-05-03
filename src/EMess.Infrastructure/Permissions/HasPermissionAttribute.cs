using Emess.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMess.Infrastructure.Permissions
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string action, string resource) =>
            Policy = Permission.NameFor(action, resource);

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any();
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
