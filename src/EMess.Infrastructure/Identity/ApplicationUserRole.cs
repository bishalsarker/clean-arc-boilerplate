using EMess.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EMess.Infrastructure.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        [Key]
        public Guid Id { get; set; }
    }
}
