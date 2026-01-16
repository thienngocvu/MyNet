using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    public class FunctionConfiguration : BaseEntityConfiguration<Function, string>
    {
        public override void Configure(EntityTypeBuilder<Function> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Functions");
            builder.Property(f => f.Id).HasMaxLength(50);
            
            builder.Property(f => f.Code).IsRequired().HasMaxLength(50);
            builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
            builder.Property(f => f.ParentId).HasMaxLength(50);
            builder.Property(f => f.Url).HasMaxLength(50);
            builder.Property(f => f.Icon).HasMaxLength(50);
            
            builder.HasIndex(f => f.Code).IsUnique();

            // Self-referencing relationship for hierarchy
            builder.HasOne(f => f.Parent)
                .WithMany(f => f.Children)
                .HasForeignKey(f => f.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting parent if children exist
        }
    }
}
