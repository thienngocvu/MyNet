using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Configurations
{
    public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken, int>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("RefreshTokens");
            
            builder.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
            builder.Property(rt => rt.CreatedByIp).HasMaxLength(50);
            builder.Property(rt => rt.RevokedByIp).HasMaxLength(50);
            builder.Property(rt => rt.ReplacedByToken).HasMaxLength(500);
            
            // Index for quick token lookup
            builder.HasIndex(rt => rt.Token);
            builder.HasIndex(rt => rt.UserId);
            
            // Relationship
            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
