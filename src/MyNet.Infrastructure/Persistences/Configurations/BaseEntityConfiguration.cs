using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            
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
