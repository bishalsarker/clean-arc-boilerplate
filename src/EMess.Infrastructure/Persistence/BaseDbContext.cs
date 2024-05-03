using EMess.Application.Interfaces.Persistence;
using EMess.Domain.Entities;
using EMess.Infrastructure.Identity;
using EMess.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMess.Infrastructure.Persistence
{
    public class BaseDbContext :
        IdentityDbContext<
            ApplicationUser,
            ApplicationRole,
            string,
            IdentityUserClaim<string>,
            ApplicationUserRole,
            IdentityUserLogin<string>,
            ApplicationRoleClaim,
            IdentityUserToken<string>>,
        IApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseDbContext(
            DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor = default!) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable(TableConstants.Users, SchemaConstants.Identity);
            modelBuilder.Entity<ApplicationRole>().ToTable(TableConstants.Roles, SchemaConstants.Identity);
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable(TableConstants.UserTokens, SchemaConstants.Identity);
            modelBuilder.Entity<ApplicationUserRole>().ToTable(TableConstants.UserRoles, SchemaConstants.Identity);
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable(TableConstants.RoleClaims, SchemaConstants.Identity);
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable(TableConstants.UserClaims, SchemaConstants.Identity);
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable(TableConstants.UserLogins, SchemaConstants.Identity);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditInfo()
        {
            var entities = ChangeTracker.Entries<IEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var utcNow = DateTime.UtcNow;
            var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Admin";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedOnUtc = utcNow;
                    entity.Entity.CreatedBy = user;
                    entity.Entity.LastModifiedOnUtc = utcNow;
                    entity.Entity.LastModifiedBy = user;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.LastModifiedOnUtc = utcNow;
                    entity.Entity.LastModifiedBy = user;
                }
            }
        }
    }
}
