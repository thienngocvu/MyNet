using MyNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNet.Application.Interfaces.Persistence
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<bool> VerifyPasswordAsync(User user, string password);
        Task<List<int>> GetUserRoleIdsAsync(int userId);
        Task<List<int>> GetUserIdsByRoleIdAsync(int roleId);
    }
}
