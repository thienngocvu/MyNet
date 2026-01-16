namespace MyNet.Application.DTOs.Request
{
    /// <summary>
    /// Login request DTO
    /// </summary>
    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
