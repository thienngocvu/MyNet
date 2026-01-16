using Microsoft.AspNetCore.Identity;

namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Application Role entity inheriting from IdentityRole
    /// </summary>
    public class Role : IdentityRole<int>, MyNet.Domain.Interfaces.IAuditableEntity
    {
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
