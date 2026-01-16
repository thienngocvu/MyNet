using Microsoft.EntityFrameworkCore;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    public class FunctionRepository : GenericRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        protected override void Update(Function source, Function destination)
        {
            destination.Name = source.Name;
            destination.SortOrder = source.SortOrder;
            destination.Code = source.Code;
            destination.ParentId = source.ParentId;
            destination.Url = source.Url;
            destination.Icon = source.Icon;
        }

        public async Task<List<Function>> GetAllOrderedAsync()
        {
            return await _dbSet.OrderBy(f => f.SortOrder).ToListAsync();
        }
    }
}
