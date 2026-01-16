using MyNet.Application.DTOs.Request;

namespace MyNet.Application.DTOs.Response
{
    public class CreateUserResponse : CreateUserRequest
    {
        public string Id { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; }
    }
}
