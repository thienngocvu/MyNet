using MyNet.Application.Common.Cache;
using MyNet.Application.Interfaces;
using MyNet.Application.Interfaces.Persistence;

namespace MyNet.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICache _cache;

        public PermissionService(IUnitOfWork unitOfWork, ICache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(int userId)
        {
            var cacheKey = $"user_perms_{userId}";
            var cachedPermissions = await _cache.GetAsync<HashSet<string>>(cacheKey);

            if (cachedPermissions != null)
            {
                return cachedPermissions;
            }

            // Get user roles via Repository
            var userRoles = await _unitOfWork.Users.GetUserRoleIdsAsync(userId);
            
            // Get permissions via Repository
            var permissions = await _unitOfWork.Permissions.GetPermissionStringsByRoleIdsAsync(userRoles);

            var permissionSet = new HashSet<string>(permissions);
            
            // Cache for 10 minutes
            await _cache.StoreAsync(cacheKey, permissionSet, TimeSpan.FromMinutes(10));

            return permissionSet;
        }

        public async Task<List<MyNet.Application.DTOs.Response.FunctionDto>> GetAllFunctionsAsync()
        {
            var functions = await _unitOfWork.Functions.GetAllOrderedAsync();
            return functions.Select(f => new MyNet.Application.DTOs.Response.FunctionDto
            {
                Id = f.Id,
                Code = f.Code,
                Name = f.Name,
                ParentId = f.ParentId
            }).ToList();
        }

        public async Task<List<MyNet.Application.DTOs.PermissionDto>> GetPermissionsByRoleIdAsync(int roleId)
        {
            var perms = await _unitOfWork.Permissions.GetByRoleIdAsync(roleId);
            return perms.Select(p => new MyNet.Application.DTOs.PermissionDto
                {
                    FunctionId = p.FunctionId,
                    Actions = new MyNet.Application.DTOs.PermissionActionsDto
                    {
                        List = p.Actions.List,
                        Read = p.Actions.Read,
                        Create = p.Actions.Create,
                        Update = p.Actions.Update,
                        Delete = p.Actions.Delete,
                        Approve = p.Actions.Approve,
                        Export = p.Actions.Export,
                        Import = p.Actions.Import
                    }
                })
                .ToList();
        }

        public async Task UpdateRolePermissionsAsync(MyNet.Application.DTOs.Request.UpdateRolePermissionsRequest request)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                // 1. Remove old permissions
                await _unitOfWork.Permissions.DeleteByRoleIdAsync(request.RoleId);
                
                // 2. Add new permissions
                if (request.Permissions != null && request.Permissions.Any())
                {
                    var newPermissions = request.Permissions.Select(p => new MyNet.Domain.Entities.Permission
                    {
                        RoleId = request.RoleId,
                        FunctionId = p.FunctionId,
                        Actions = new MyNet.Domain.Entities.PermissionActions
                        {
                            List = p.Actions.List,
                            Read = p.Actions.Read,
                            Create = p.Actions.Create,
                            Update = p.Actions.Update,
                            Delete = p.Actions.Delete,
                            Approve = p.Actions.Approve,
                            Export = p.Actions.Export,
                            Import = p.Actions.Import
                        }
                    }).ToList();
                    
                    await _unitOfWork.Permissions.AddRangeAsync(newPermissions);
                }
                
                await _unitOfWork.CommitChangesAsync();

                // 3. Invalidate cache for users in this role
                var userIds = await _unitOfWork.Users.GetUserIdsByRoleIdAsync(request.RoleId);

                foreach (var userId in userIds)
                {
                   await _cache.DeleteAsync($"user_perms_{userId}");
                }
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
