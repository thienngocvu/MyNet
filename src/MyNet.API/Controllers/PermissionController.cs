using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNet.Application.Common;
using MyNet.Application.Common.Attributes;
using MyNet.Application.DTOs.Request;
using MyNet.Application.Interfaces;

namespace MyNet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("functions")]
        [HasPermission("ROLES", "VIEW")]
        public async Task<IActionResult> GetAllFunctions()
        {
            var result = await _permissionService.GetAllFunctionsAsync();
            return Ok(result);
        }

        [HttpGet("role/{roleId}")]
        [HasPermission("ROLES", "VIEW")]
        public async Task<IActionResult> GetRolePermissions(int roleId)
        {
            var result = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            return Ok(result);
        }

        [HttpPost("role")]
        [HasPermission("ROLES", "UPDATE")]
        public async Task<IActionResult> UpdateRolePermissions([FromBody] UpdateRolePermissionsRequest request)
        {
            await _permissionService.UpdateRolePermissionsAsync(request);
            return Ok(new { message = "Permissions updated successfully" });
        }
    }
}
