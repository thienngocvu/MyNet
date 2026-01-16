using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    public class LoginLogConfiguration : BaseEntityConfiguration<LoginLog, int>
    {
        public override void Configure(EntityTypeBuilder<LoginLog> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("LoginLogs");
            
            builder.Property(ll => ll.RemoteAddress).IsRequired().HasMaxLength(50);
            builder.Property(ll => ll.LoginDate).IsRequired();
            
            // Index for quick lookup
            builder.HasIndex(ll => ll.UserId);
            builder.HasIndex(ll => ll.LoginDate);
            
            // Relationship
            builder.HasOne(ll => ll.User)
                .WithMany()
                .HasForeignKey(ll => ll.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
