using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMess.Domain.Entities
{
    public interface IEntity
    {
        object Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
