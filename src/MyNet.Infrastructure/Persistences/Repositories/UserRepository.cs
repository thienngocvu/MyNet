using Microsoft.EntityFrameworkCore;
using MyNet.Domain.Entities;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Infrastructure.Persistences;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    /// <summary>
    /// User repository implementation for ASP.NET Identity User entity
    /// Note: User inherits from IdentityUser<int>, not BaseEntity, so we implement IRepository directly
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public IQueryable<User> AsQueryable() => _dbSet.AsQueryable();

        public async Task AddAsync(User e) => await _dbSet.AddAsync(e);

        public void Add(User e) => _dbSet.Add(e);

        public async Task AddRangeAsync(IEnumerable<User> entities) => await _dbSet.AddRangeAsync(entities);

        public async Task<User?> FindAsync(int id) => await _dbSet.FindAsync(id);

        public void Remove(User e) => _dbSet.Remove(e);

        public void RemoveRange(IEnumerable<User> entities) => _dbSet.RemoveRange(entities);

        public void UpdateRange(IEnumerable<User> entities) => _dbSet.UpdateRange(entities);

        public async Task<User> UpdateAsync(int key, User e)
        {
            var trackingEntity = await FindAsync(key);
            if (trackingEntity != null)
            {
                // Update properties
                trackingEntity.UserName = e.UserName;
                trackingEntity.Email = e.Email;
                trackingEntity.IsActive = e.IsActive;
                trackingEntity.LastModifiedAt = DateTime.UtcNow;
            }
            return trackingEntity ?? e;
        }

        public async Task<IEnumerable<User>> AddOrUpdateRangeAsync(IEnumerable<User> entities)
        {
            var ids = entities.Select(x => x.Id).ToList();
            var listExists = await _dbSet.AsQueryable().Where(x => ids.Contains(x.Id)).ToListAsync();
            var existIds = listExists.Select(x => x.Id).ToList();
            var listNotExists = entities.Where(x => !existIds.Contains(x.Id)).ToList();

            foreach (var item in listExists)
            {
                var sourceEntity = entities.FirstOrDefault(x => x.Id == item.Id);
                if (sourceEntity != null)
                {
                    await UpdateAsync(item.Id, sourceEntity);
                }
            }

            if (listNotExists.Any())
            {
                await _dbSet.AddRangeAsync(listNotExists);
            }

            return entities;
        }

        public async Task<List<int>> GetUserRoleIdsAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();
        }

        public Task<bool> VerifyPasswordAsync(User user, string password)
        {
            throw new NotImplementedException("Use UserManager to verify password.");
        }

        public async Task<List<int>> GetUserIdsByRoleIdAsync(int roleId)
        {
            return await _context.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Select(ur => ur.UserId)
                .ToListAsync();
        }
    }
}
