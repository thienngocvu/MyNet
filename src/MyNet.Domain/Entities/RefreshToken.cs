namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Refresh token entity for JWT token refresh mechanism
    /// </summary>
    public class RefreshToken : BaseEntity<int>
    {
        public required string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        // CreatedAt is inherited from BaseEntity
        public string? CreatedByIp { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        
        /// <summary>
        /// Check if the token has expired
        /// </summary>
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        
        /// <summary>
        /// Check if the token has been revoked
        /// </summary>
        public bool IsRevoked => RevokedAt != null;
        
        /// <summary>
        /// Check if the token is still active (not expired and not revoked)
        /// </summary>
        public bool IsActive => !IsRevoked && !IsExpired;
        
        /// <summary>
        /// Navigation property to User
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}
