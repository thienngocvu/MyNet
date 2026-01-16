using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyNet.Domain.Entities;
using MyNet.Infrastructure.Persistences.Configurations;

using MyNet.Application.Interfaces;
using MyNet.Domain.Interfaces;

namespace MyNet.Infrastructure.Persistences
{
    /// <summary>
    /// Application database context with ASP.NET Identity support
    /// </summary>
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly ICurrentUserService? _currentUserService;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService? currentUserService = null) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService?.UserId;

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                        
                    case EntityState.Deleted:
                        // Convert Hard Delete to Soft Delete
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.DeletedBy = userId;
                        // entry.Entity.LastModifiedAt = DateTime.UtcNow; // Optional
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region "Core Application Entities"
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Customize Identity table names
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            // Configure RefreshToken entity
            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasOne(e => e.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Apply all entity configurations from the assembly
            var assembly = typeof(BaseEntityConfiguration<,>).Assembly;
            builder.ApplyConfigurationsFromAssembly(assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable lazy loading proxies for navigation properties
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
