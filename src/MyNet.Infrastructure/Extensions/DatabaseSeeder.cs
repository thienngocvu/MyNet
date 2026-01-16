using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyNet.Domain.Entities;
using MyNet.Infrastructure.Persistences;

namespace MyNet.Infrastructure.Extensions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<AppDbContext>>();

            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();

                // Apply pending migrations
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully.");

                // Seed roles
                await SeedRolesAsync(roleManager, logger);

                // Seed admin user
                await SeedAdminUserAsync(userManager, logger);

                // Seed permissions
                await SeedPermissionsAsync(context, roleManager, logger);

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<Role> roleManager, ILogger logger)
        {
            var roles = new[]
            {
                new Role { Name = "Admin", Description = "Administrator with full access", CreatedAt = DateTime.UtcNow },
                new Role { Name = "User", Description = "Standard user with limited access", CreatedAt = DateTime.UtcNow }
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Created role: {RoleName}", role.Name);
                    }
                    else
                    {
                        logger.LogWarning("Failed to create role {RoleName}: {Errors}", 
                            role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<User> userManager, ILogger logger)
        {
            const string adminUserName = "admin";
            const string adminEmail = "admin@mynet.local";
            const string adminPassword = "H1234@56"; // Change this in production!

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    logger.LogInformation("Created admin user: {UserName}", adminUserName);
                }
                else
                {
                    logger.LogWarning("Failed to create admin user: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    return;
                }
            }
            else
            {
                logger.LogInformation("Admin user already exists.");
            }

            // Ensure Admin user has Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Assigned 'Admin' role to user '{UserName}'", adminUserName);
            }
        }

        private static async Task SeedPermissionsAsync(AppDbContext context, RoleManager<Role> roleManager, ILogger logger)
        {
            // 1. Seed Functions
            if (!await context.Functions.AnyAsync())
            {
                var functions = new[]
                {
                    new Function { Id = Guid.NewGuid().ToString(), Code = "USERS", Name = "User Management", SortOrder = 1 },
                    new Function { Id = Guid.NewGuid().ToString(), Code = "ROLES", Name = "Role Management", SortOrder = 2 }
                };

                await context.Functions.AddRangeAsync(functions);
                await context.SaveChangesAsync();
                logger.LogInformation($"Seeded {functions.Length} functions.");
            }

            // 2. Seed Permissions for Admin
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var actions = new[] { "VIEW", "CREATE", "UPDATE", "DELETE" };
                var permissions = new List<Permission>();
                var allFunctions = await context.Functions.ToListAsync();

                foreach (var func in allFunctions)
                {
                    foreach (var action in actions)
                    {
                        if (!await context.Permissions.AnyAsync(p => p.RoleId == adminRole.Id && p.FunctionId == func.Id && p.ActionId == action))
                        {
                            permissions.Add(new Permission
                            {
                                RoleId = adminRole.Id,
                                FunctionId = func.Id,
                                ActionId = action
                            });
                        }
                    }
                }

                if (permissions.Any())
                {
                    await context.Permissions.AddRangeAsync(permissions);
                    await context.SaveChangesAsync();
                    logger.LogInformation($"Seeded {permissions.Count} permissions for Admin role.");
                }
            }
        }
    }
}
