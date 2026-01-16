using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet.Application.Common;
using MyNet.Application.Common.Attributes;
using MyNet.Application.DTOs.Request;
using MyNet.Application.Services;

namespace MyNet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [HasPermission("USERS", "CREATE")]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest command, CancellationToken cancellationToken)
        {
            var response = await _userService.CreateUserAsync(command, cancellationToken);
            return Ok(response);
        }

        [HttpGet("info")]
        [Authorize(Policy = AuthzPolicy.USER_POLICY)]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _userService.GetInfoAsync();
            return Ok(response);
        }
    }
}
