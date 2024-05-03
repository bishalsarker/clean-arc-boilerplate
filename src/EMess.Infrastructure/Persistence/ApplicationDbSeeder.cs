using Emess.Shared.Authorization;
using EMess.Infrastructure.Identity;
using EMess.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMess.Infrastructure.Persistence
{
    internal class ApplicationDbSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationDbSeeder> _logger;

        public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<ApplicationDbSeeder> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
        }

        private async Task SeedRolesAsync(ApplicationDbContext dbContext)
        {
            foreach (string roleName in Roles.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName) is not ApplicationRole role)
                {
                    _logger.LogInformation("Seeding {role} Role", roleName);

                    role = new ApplicationRole(roleName, $"{roleName} Role");

                    await _roleManager.CreateAsync(role);
                }

                var permissions = new List<Permission>();

                if (roleName == Roles.Admin)
                {
                    permissions.AddRange(PermissionCollection.Admin);
                }
                else
                {
                    permissions.AddRange(PermissionCollection.Basic);
                }

                await AssignPermissionsToRoleAsync(dbContext, permissions, role);
            }
        }

        private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<Permission> permissions, ApplicationRole role)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == Claims.Permission && c.Value == permission.Name))
                {
                    _logger.LogInformation("Seeding {role} Permission '{permission}'", role.Name, permission.Name);

                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = Claims.Permission,
                        ClaimValue = permission.Name,
                        CreatedBy = "ApplicationDbSeeder"
                    });

                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == "admin@root.com") is null) 
            {
                foreach (string roleName in new List<string> { Roles.Admin })
                {
                    string userName = $"{roleName}".ToLowerInvariant();

                    string userEmail = "admin@root.com";

                    var adminUser = new ApplicationUser
                    {
                        FirstName = roleName,
                        LastName = string.Empty,
                        Email = userEmail,
                        UserName = userName,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        NormalizedEmail = userEmail?.ToUpperInvariant(),
                        NormalizedUserName = userName.ToUpperInvariant(),
                        IsActive = true
                    };

                    _logger.LogInformation($"Seeding default user");

                    var password = new PasswordHasher<ApplicationUser>();

                    adminUser.PasswordHash = password.HashPassword(adminUser, "123456Aa!!");

                    await _userManager.CreateAsync(adminUser);

                    if (!await _userManager.IsInRoleAsync(adminUser, roleName))
                    {
                        _logger.LogInformation($"Assigning role to user");

                        await _userManager.AddToRoleAsync(adminUser, roleName);
                    }
                }
            }
        }
    }
}
