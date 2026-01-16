namespace MyNet.Application.DTOs.Request
{
    public class CreateUserRequest
    {
        public required string UserName { get; set; }
        
        public required string Email { get; set; }
        
        public required string Password { get; set; }
        
        public string? Name { get; set; }
        
        public bool Active { get; set; } = true;
    }
}
