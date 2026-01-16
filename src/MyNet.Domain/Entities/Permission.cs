using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNet.Domain.Entities
{
    [Table("Permissions")]
    public class Permission : BaseEntity<int>
    {
        public int RoleId { get; set; }
        
        [MaxLength(50)]
        public string FunctionId { get; set; } = null!;
        
        [MaxLength(50)]
        public string ActionId { get; set; } = null!; // VIEW, CREATE, UPDATE, DELETE, APPROVE...

        public virtual Role Role { get; set; } = null!;
        public virtual Function Function { get; set; } = null!;
    }
}
