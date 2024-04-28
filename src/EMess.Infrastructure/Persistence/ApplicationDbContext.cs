using EMess.Application.Interfaces.Persistence;
using EMess.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMess.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable(TableConstants.Users, SchemaConstants.Authentication);
            modelBuilder.Entity<IdentityRole>().ToTable(TableConstants.Roles, SchemaConstants.Authentication);
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable(TableConstants.UserTokens, SchemaConstants.Authentication).HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable(TableConstants.UserRoles, SchemaConstants.Authentication).HasNoKey();
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable(TableConstants.RoleClaims, SchemaConstants.Authentication).HasNoKey();
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable(TableConstants.UserClaims, SchemaConstants.Authentication).HasNoKey();
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable(TableConstants.UserLogins, SchemaConstants.Authentication).HasNoKey();
        }
    }
}
