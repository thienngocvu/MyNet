using Microsoft.AspNetCore.Identity;
using MyNet.Domain.Interfaces;

namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Application User entity inheriting from IdentityUser with custom properties
    /// </summary>
    public class User : IdentityUser<int>, IEntity<int>, IAuditableEntity
    {
        
        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Soft delete fields
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        
        // Navigation properties
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
