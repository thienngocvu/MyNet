using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    /// <summary>
    /// Role entity configuration for ASP.NET Identity Role
    /// Note: Basic Identity columns are configured by IdentityDbContext
    /// This adds custom property mappings
    /// </summary>
    public class RoleConfiguration : AuditableEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
            
            builder.Property(r => r.Description).HasMaxLength(500);
        }
    }
}
