using EMess.Application.Common.Models;

namespace EMess.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}