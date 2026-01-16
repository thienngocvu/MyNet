
namespace MyNet.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(int userId);
        
        // Management methods
        Task<List<MyNet.Application.DTOs.Response.FunctionDto>> GetAllFunctionsAsync();
        Task<List<MyNet.Application.DTOs.PermissionDto>> GetPermissionsByRoleIdAsync(int roleId);
        Task UpdateRolePermissionsAsync(MyNet.Application.DTOs.Request.UpdateRolePermissionsRequest request);
    }
}
