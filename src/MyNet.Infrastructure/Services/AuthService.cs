using MyNet.Domain.Entities;
using MyNet.Application.Common.Enums;
using MyNet.Application.Services;
using MyNet.Application.Interfaces.Persistence;

namespace MyNet.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLoginLogAsync(string userId, string remoteAddress, LoginResultEnum loginResult)
        {
            var entity = new LoginLog()
            {
                RemoteAddress = remoteAddress,
                UserId = userId,
                LoginResult = (short)loginResult,
                LoginDate = DateTime.Now,
            };

            await _unitOfWork.LoginLogs.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
