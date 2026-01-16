using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyNet.Application.DTOs.Request;
using MyNet.Application.DTOs.Response;
using MyNet.Application.Interfaces;
using MyNet.Domain.Entities;
using MyNet.Application.Services;
using MyNet.Application.Common;
using MyNet.Application.Common.Enums;

namespace MyNet.API.Controllers
{
    /// <summary>
    /// Authentication controller for JWT-based authentication
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthService _authService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _authService = authService;
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    // Cannot log failed attempt without userId - user doesn't exist
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    await _authService.AddLoginLogAsync(user.Id, GetClientIp(), LoginResultEnum.LOGIN_FAILED);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                if (!user.IsActive)
                {
                    return Unauthorized(new { message = "User account is disabled" });
                }

                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _jwtTokenService.GenerateAccessToken(user, roles);
                var refreshToken = _jwtTokenService.GenerateRefreshToken(user.Id, GetClientIp());

                await _refreshTokenRepository.AddAsync(refreshToken);
                await _refreshTokenRepository.SaveChangesAsync();

                await _authService.AddLoginLogAsync(user.Id, GetClientIp(), LoginResultEnum.LOGIN_SUCCESSFULLY);

                return Ok(new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token,
                    ExpiresIn = 3600, // 60 minutes in seconds
                    TokenType = "Bearer"
                });
            }
            catch (Exception)
            {
                // Cannot log failed attempt without userId in exception handler
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Refresh access token using refresh token
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            
            if (storedToken == null || !storedToken.IsActive)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { message = "User not found or disabled" });
            }

            // Revoke the old refresh token
            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = GetClientIp();

            // Generate new tokens
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtTokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(user.Id, GetClientIp());
            storedToken.ReplacedByToken = newRefreshToken.Token;

            _refreshTokenRepository.Update(storedToken);
            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return Ok(new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }

        /// <summary>
        /// Logout and revoke refresh token
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest? request)
        {
            if (request != null && !string.IsNullOrEmpty(request.RefreshToken))
            {
                var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                
                if (storedToken != null)
                {
                    storedToken.RevokedAt = DateTime.UtcNow;
                    storedToken.RevokedByIp = GetClientIp();
                    _refreshTokenRepository.Update(storedToken);
                    await _refreshTokenRepository.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Logged out successfully" });
        }

        /// <summary>
        /// Get current authenticated user info
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = roles
            });
        }

        /// <summary>
        /// Revoke all refresh tokens for a user (admin function)
        /// </summary>
        [Authorize(Policy = AuthzPolicy.ADMIN_POLICY)]
        [HttpPost("revoke-all/{userId}")]
        public async Task<IActionResult> RevokeAllTokens(int userId)
        {
            var tokens = await _refreshTokenRepository.GetByUserIdAsync(userId);
            
            foreach (var token in tokens.Where(t => t.IsActive))
            {
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIp = GetClientIp();
                _refreshTokenRepository.Update(token);
            }

            await _refreshTokenRepository.SaveChangesAsync();

            return Ok(new { message = "All tokens revoked successfully" });
        }

        #region Private Methods

        private string GetClientIp()
        {
            var clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (!string.IsNullOrEmpty(clientIpAddress) && clientIpAddress.StartsWith("::ffff:"))
            {
                clientIpAddress = clientIpAddress.Replace("::ffff:", "");
            }

            if (clientIpAddress is "::1" or "127.0.0.1")
            {
                var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    clientIpAddress = forwardedFor.Split(',').FirstOrDefault()?.Trim();
                }
            }

            return clientIpAddress ?? "unknown";
        }

        #endregion
    }
}
