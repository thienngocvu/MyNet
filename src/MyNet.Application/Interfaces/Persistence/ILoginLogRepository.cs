using MyNet.Domain.Entities;

namespace MyNet.Application.Interfaces.Persistence
{
    public interface ILoginLogRepository
    {
        Task AddAsync(LoginLog entity);
    }
}
