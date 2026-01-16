using MyNet.Domain.Entities;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Infrastructure.Persistences;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    public class LoginLogRepository : ILoginLogRepository
    {
        private readonly AppDbContext _context;

        public LoginLogRepository(AppDbContext context) { 
            _context = context;
        }

        public async Task AddAsync(LoginLog entity)
        {
            await _context.AddAsync(entity);
        }
    }
}
