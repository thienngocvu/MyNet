namespace MyNet.Application.DTOs.Response
{
    /// <summary>
    /// Login response DTO containing JWT tokens
    /// </summary>
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
