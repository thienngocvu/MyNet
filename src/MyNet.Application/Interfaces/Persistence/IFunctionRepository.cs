using MyNet.Domain.Entities;

namespace MyNet.Application.Interfaces.Persistence
{
    public interface IFunctionRepository : IRepository<Function, string>
    {
        Task<List<Function>> GetAllOrderedAsync();
    }
}
