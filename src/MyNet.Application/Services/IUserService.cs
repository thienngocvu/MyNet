using MyNet.Application.DTOs.Request;
using MyNet.Application.DTOs.Response;

namespace MyNet.Application.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken token);
        Task<UserLoginResponse> FindByUserNameAsync(string userName);
        Task<UserDto> FindlByUserIdAsync(string userId);
        Task<UserDto> GetInfoAsync();
    }
}
