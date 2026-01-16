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
            
            builder.Property(p => p.FunctionId).IsRequired().HasMaxLength(50);
            
            // Configure Actions as JSONB column in PostgreSQL
            builder.OwnsOne(p => p.Actions, actionsBuilder =>
            {
                actionsBuilder.ToJson("Actions");
            });
            
            // Unique Index: One permission record per Role-Function combination
            builder.HasIndex(p => new { p.RoleId, p.FunctionId }).IsUnique();

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
