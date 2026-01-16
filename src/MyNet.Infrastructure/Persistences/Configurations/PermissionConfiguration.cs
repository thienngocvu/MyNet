using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    public class PermissionConfiguration : BaseEntityConfiguration<Permission, int>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Permissions");
            
            // Unique Index for business key
            builder.HasIndex(p => new { p.RoleId, p.FunctionId, p.ActionId }).IsUnique();

            // Relationships
            builder.HasOne(p => p.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Function)
                .WithMany(f => f.Permissions)
                .HasForeignKey(p => p.FunctionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
