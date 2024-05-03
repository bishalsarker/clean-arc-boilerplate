using System.ComponentModel.DataAnnotations;

namespace EMess.Domain.Entities
{
    public abstract class BaseEntity<T> : IEntity
    {
        [Required]
        public T Id { get; set; } = default!;

        object IEntity.Id
        {
            get { return Id; }
            set { }
        }

        [Required]
        public string CreatedBy { get; set; } = default!;

        [Required]
        public DateTime CreatedOnUtc { get; set; }

        public string LastModifiedBy { get; set; } = default!;

        public DateTime? LastModifiedOnUtc { get; set; }

        public bool IsDeleted { get; set; }

        public BaseEntity()
        {

        }
    }
}
