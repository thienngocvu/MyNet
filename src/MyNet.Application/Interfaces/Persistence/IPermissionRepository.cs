using MyNet.Domain.Entities;

namespace MyNet.Application.Interfaces.Persistence
{
    public interface IPermissionRepository : IRepository<Permission, int>
    {
        Task<List<string>> GetPermissionStringsByRoleIdsAsync(IEnumerable<int> roleIds);
        Task<List<Permission>> GetByRoleIdAsync(int roleId);
        Task DeleteByRoleIdAsync(int roleId);
    }
}
