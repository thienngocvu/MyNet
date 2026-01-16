using Microsoft.AspNetCore.Identity;
using MyNet.Domain.Interfaces;

namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Junction table for User-Role many-to-many relationship
    /// Inherits from IdentityUserRole to integrate with ASP.NET Identity
    /// </summary>
    public class UserRole : IdentityUserRole<int>, IAuditableEntity
    {
        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        
        // Soft delete fields
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
