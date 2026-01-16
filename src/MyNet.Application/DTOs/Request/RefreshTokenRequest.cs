namespace MyNet.Application.DTOs.Request
{
    /// <summary>
    /// Refresh token request DTO
    /// </summary>
    public class RefreshTokenRequest
    {
        public required string RefreshToken { get; set; }
    }
}
