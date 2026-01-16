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
            destination.Actions = source.Actions;
        }

        public async Task<List<string>> GetPermissionStringsByRoleIdsAsync(IEnumerable<int> roleIds)
        {
            var permissions = await _dbSet
                .Include(p => p.Function)
                .Where(p => roleIds.Contains(p.RoleId))
                .ToListAsync();
            
            // Generate permission strings based on Actions object
            var result = new List<string>();
            foreach (var p in permissions)
            {
                if (p.Actions.List) result.Add($"{p.Function.Code}.LIST");
                if (p.Actions.Read) result.Add($"{p.Function.Code}.READ");
                if (p.Actions.Create) result.Add($"{p.Function.Code}.CREATE");
                if (p.Actions.Update) result.Add($"{p.Function.Code}.UPDATE");
                if (p.Actions.Delete) result.Add($"{p.Function.Code}.DELETE");
                if (p.Actions.Approve) result.Add($"{p.Function.Code}.APPROVE");
                if (p.Actions.Export) result.Add($"{p.Function.Code}.EXPORT");
                if (p.Actions.Import) result.Add($"{p.Function.Code}.IMPORT");
            }
            return result;
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
