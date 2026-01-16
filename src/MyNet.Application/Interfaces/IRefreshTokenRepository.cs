using MyNet.Domain.Entities;

namespace MyNet.Application.Interfaces
{
    /// <summary>
    /// Interface for refresh token repository operations
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Get refresh token by token string
        /// </summary>
        Task<RefreshToken?> GetByTokenAsync(string token);
        
        /// <summary>
        /// Get all refresh tokens for a user
        /// </summary>
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId);
        
        /// <summary>
        /// Add a new refresh token
        /// </summary>
        Task AddAsync(RefreshToken refreshToken);
        
        /// <summary>
        /// Update an existing refresh token
        /// </summary>
        void Update(RefreshToken refreshToken);
        
        /// <summary>
        /// Remove expired and revoked tokens for a user
        /// </summary>
        Task RemoveOldTokensAsync(int userId, int keepDays = 2);
        
        /// <summary>
        /// Save changes to database
        /// </summary>
        Task SaveChangesAsync();
    }
}
