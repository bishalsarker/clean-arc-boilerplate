using EMess.Domain.Products;
using EMess.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace EMess.Infrastructure.Persistence
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().ToTable(TableConstants.Products, SchemaConstants.Catalog);
        }
    }
}
