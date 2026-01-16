using MyNet.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNet.Domain.Entities
{
    [Table("Functions")]
    public class Function : BaseEntity<string>
    {

        [MaxLength(50)]
        public string Code { get; set; } = null!; // SYSTEM, USERS, ROLES...

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? ParentId { get; set; }

        public int SortOrder { get; set; }

        [MaxLength(50)]
        public string? Url { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        public virtual Function? Parent { get; set; }
        public virtual ICollection<Function> Children { get; set; } = new List<Function>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
