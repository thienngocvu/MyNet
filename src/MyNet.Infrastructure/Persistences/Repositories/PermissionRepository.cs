using Microsoft.EntityFrameworkCore;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    public class PermissionRepository : GenericRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        protected override void Update(Permission source, Permission destination)
        {
            destination.RoleId = source.RoleId;
            destination.FunctionId = source.FunctionId;
            destination.ActionId = source.ActionId;
        }

        public async Task<List<string>> GetPermissionStringsByRoleIdsAsync(IEnumerable<int> roleIds)
        {
            return await _dbSet
                .Include(p => p.Function)
                .Where(p => roleIds.Contains(p.RoleId))
                .Select(p => $"{p.Function.Code}.{p.ActionId}")
                .ToListAsync();
        }

        public async Task<List<Permission>> GetByRoleIdAsync(int roleId)
        {
            return await _dbSet
                .Where(p => p.RoleId == roleId)
                .ToListAsync();
        }

        public async Task DeleteByRoleIdAsync(int roleId)
        {
            var items = await _dbSet.Where(p => p.RoleId == roleId).ToListAsync();
            _dbSet.RemoveRange(items);
        }
    }
}
