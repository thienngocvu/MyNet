using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNet.Domain.Entities;
using MyNet.Application.Services;
using MyNet.Application.DTOs.Request;
using MyNet.Application.DTOs.Response;
using MyNet.Application.Interfaces.Persistence;

namespace MyNet.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IUserContext _userContext;
        private readonly UserManager<User> _userManager;

        public UserService(
            IUnitOfWork unitOfWork, 
            ILogger<UserService> logger,
            IMapper mapper, 
            IUserContext userContext,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userContext = userContext;
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken token)
        {
            _logger.LogInformation($"{nameof(CreateUserAsync)} - Begin...");
            
            var entity = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            
            // Use Identity's password hashing
            var result = await _userManager.CreateAsync(entity, request.Password ?? "DefaultPass123!");
            
            if (!result.Succeeded)
            {
                _logger.LogError($"{nameof(CreateUserAsync)} - Failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            
            _logger.LogInformation($"{nameof(CreateUserAsync)} - Done.");

            return _mapper.Map<CreateUserResponse>(entity);
        }

        public async Task<UserLoginResponse> FindByUserNameAsync(string userName)
        {
            _logger.LogInformation($"{nameof(FindByUserNameAsync)} - Begin...");
            var entity = await _unitOfWork.Users.AsQueryable()
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();

            return UserLoginResponse.Create(entity!)!;
        }

        public async Task<UserDto> FindlByUserIdAsync(string userId)
        {
            _logger.LogInformation($"{nameof(FindlByUserIdAsync)} - Begin...");
            
            if (!int.TryParse(userId, out int id))
            {
                _logger.LogWarning($"{nameof(FindlByUserIdAsync)} - Invalid userId: {userId}");
                return null!;
            }
            
            var entity = await _unitOfWork.Users.AsQueryable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"{nameof(FindlByUserIdAsync)} - Done.");
            return UserDto.Create(entity!)!;
        }

        public async Task<UserDto> GetInfoAsync()
        {
            _logger.LogInformation($"{nameof(GetInfoAsync)} - Begin...");
            
            if (!int.TryParse(_userContext.Id, out int id))
            {
                _logger.LogWarning($"{nameof(GetInfoAsync)} - Invalid userId from context: {_userContext.Id}");
                return null!;
            }
            
            var entity = await _unitOfWork.Users.AsQueryable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"{nameof(GetInfoAsync)} - Done.");
            return UserDto.Create(entity!)!;
        }
    }
}
