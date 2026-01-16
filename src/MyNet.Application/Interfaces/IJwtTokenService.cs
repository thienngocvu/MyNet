using MyNet.Domain.Entities;
using System.Security.Claims;

namespace MyNet.Application.Interfaces
{
    /// <summary>
    /// Interface for JWT token generation and validation
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generate an access token for the specified user
        /// </summary>
        string GenerateAccessToken(User user, IList<string> roles);
        
        /// <summary>
        /// Generate a refresh token for the specified user
        /// </summary>
        RefreshToken GenerateRefreshToken(int userId, string? ipAddress);
        
        /// <summary>
        /// Validate the specified token and return claims principal
        /// </summary>
        ClaimsPrincipal? ValidateToken(string token);
    }
}
