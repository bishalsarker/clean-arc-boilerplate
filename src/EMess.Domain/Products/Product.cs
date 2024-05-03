using EMess.Domain.Entities;

namespace EMess.Domain.Products
{
    public class Product : BaseEntity<Guid>, IEntity
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
