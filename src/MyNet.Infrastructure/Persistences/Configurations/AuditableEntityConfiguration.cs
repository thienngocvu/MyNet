using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Interfaces;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    /// <summary>
    /// Base configuration for entities that implement IAuditableEntity but don't inherit from BaseEntity
    /// (e.g., Identity entities like User, Role, UserRole)
    /// </summary>
    public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IAuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Audit configuration
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired(false);
            builder.Property(e => e.LastModifiedAt).IsRequired(false);
            builder.Property(e => e.LastModifiedBy).IsRequired(false);
            
            // Soft delete configuration
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.DeletedAt).IsRequired(false);
            builder.Property(e => e.DeletedBy).IsRequired(false);

            // Global Query Filter for Soft Delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
