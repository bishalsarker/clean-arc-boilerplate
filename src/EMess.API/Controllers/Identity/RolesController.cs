using Emess.Shared.Authorization;
using EMess.Application.Common.Interfaces;
using EMess.Application.Identity.Roles;
using EMess.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Action = Emess.Shared.Authorization.Action;

namespace EMess.API.Controllers.Identity;
public class RolesController : BaseController
{
    private readonly IRoleService _roleService;
    private readonly ICurrentUser _currentUser;

    public RolesController(IRoleService roleService, ICurrentUser currentUser) => (_roleService, _currentUser) = (roleService, currentUser);

    [HttpGet]
    [HasPermission(Action.View, Resource.Roles)]
    public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _roleService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [HasPermission(Action.View, Resource.Roles)]
    public Task<RoleDto> GetByIdAsync(string id)
    {
        return _roleService.GetByIdAsync(id);
    }

    [HttpGet("{id}/permissions")]
    [HasPermission(Action.View, Resource.RoleClaims)]
    public Task<RoleDto> GetByIdWithPermissionsAsync(string id, CancellationToken cancellationToken)
    {
        return _roleService.GetByIdWithPermissionsAsync(id, cancellationToken);
    }

    [HttpPut("{id}/permissions")]
    [HasPermission(Action.Update, Resource.RoleClaims)]
    public async Task<ActionResult<string>> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        if (id != request.RoleId)
        {
            return BadRequest();
        }

        return Ok(await _roleService.UpdatePermissionsAsync(request, cancellationToken));
    }

    [HttpPost]
    [HasPermission(Action.Create, Resource.Roles)]
    public Task<string> RegisterRoleAsync(CreateOrUpdateRoleRequest request)
    {
        return _roleService.CreateOrUpdateAsync(request);
    }

    [HttpDelete("{id}")]
    [HasPermission(Action.Delete, Resource.Roles)]
    public Task<string> DeleteAsync(string id)
    {
        return _roleService.DeleteAsync(id);
    }
}