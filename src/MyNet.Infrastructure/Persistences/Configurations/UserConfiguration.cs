using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    /// <summary>
    /// User entity configuration for ASP.NET Identity User
    /// Note: Basic Identity columns are configured by IdentityDbContext
    /// This adds custom property mappings
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Audit fields
            // Audit fields configuration matching BaseEntityConfiguration
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired(false);
            builder.Property(e => e.LastModifiedAt).IsRequired(false);
            builder.Property(e => e.LastModifiedBy).IsRequired(false);
            
            builder.Property(e => e.IsActive).HasDefaultValue(true);

            // Soft delete configuration
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.DeletedAt).IsRequired(false);
            builder.Property(e => e.DeletedBy).IsRequired(false);

            // Global Query Filter for Soft Delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
