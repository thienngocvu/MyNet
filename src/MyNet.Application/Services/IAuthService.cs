using MyNet.Application.Common.Enums;

namespace MyNet.Application.Services
{
    public interface IAuthService
    {
        Task AddLoginLogAsync(int userId, string remoteAddress, LoginResultEnum loginResult);
    }
}
