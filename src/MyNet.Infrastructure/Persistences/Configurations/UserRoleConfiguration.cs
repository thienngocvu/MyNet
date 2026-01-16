using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    /// <summary>
    /// UserRole entity configuration for User-Role junction table
    /// Note: Cannot use HasQueryFilter because IdentityUserRole is the root entity type
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Audit fields configuration (without Query Filter for Identity entities)
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired(false);
            builder.Property(e => e.LastModifiedAt).IsRequired(false);
            builder.Property(e => e.LastModifiedBy).IsRequired(false);
            
            // Soft delete configuration
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.DeletedAt).IsRequired(false);
            builder.Property(e => e.DeletedBy).IsRequired(false);
            
            // Relationships
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            
            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }
    }
}
