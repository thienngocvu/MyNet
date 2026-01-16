using MyNet.Application.Common.Enums;

namespace MyNet.Application.Services
{
    public interface IAuthService
    {
        Task AddLoginLogAsync(string userId, string remoteAddress, LoginResultEnum loginResult);
    }
}
