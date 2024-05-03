using EMess.Application.Common.Exceptions;
using EMess.Shared.Authorization;
using EMess.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace EMess.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new UnauthorizedException("Authentication Failed.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
            .Where(r => userRoles.Contains(r.Name!))
            .ToListAsync(cancellationToken))
        {
            var claims = await _roleClaimRepository
                .GetMultiple($"SELECT * FROM \"{SchemaConstants.Identity}\".\"{TableConstants.RoleClaims}\" WHERE \"RoleId\" = @roleId", 
                new { roleId = role.Id });

            permissions.AddRange(claims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == Claims.Permission)
                .Select(rc => rc.ClaimValue!));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
    {
        var permissions = await GetPermissionsAsync(userId, cancellationToken);
        return permissions?.Contains(permission) ?? false;
    }
}