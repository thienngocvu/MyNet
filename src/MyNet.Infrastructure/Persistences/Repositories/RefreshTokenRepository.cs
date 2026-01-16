using Microsoft.EntityFrameworkCore;
using MyNet.Application.Interfaces;
using MyNet.Domain.Entities;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
        }

        public async Task RemoveOldTokensAsync(int userId, int keepDays = 2)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-keepDays);
            var oldTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && (t.ExpiresAt < DateTime.UtcNow || t.IsRevoked || t.CreatedAt < cutoffDate))
                .ToListAsync();

            if (oldTokens.Any())
            {
                _context.RefreshTokens.RemoveRange(oldTokens);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
