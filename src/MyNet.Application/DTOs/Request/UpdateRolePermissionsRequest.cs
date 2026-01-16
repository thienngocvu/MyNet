using System.ComponentModel.DataAnnotations;

namespace MyNet.Application.DTOs.Request
{
    public class UpdateRolePermissionsRequest
    {
        [Required]
        public int RoleId { get; set; }
        
        [Required]
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
