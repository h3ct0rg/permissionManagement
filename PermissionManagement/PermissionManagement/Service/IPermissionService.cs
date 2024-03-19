using PermissionManagement.Model;
using PermissionManagement.ViewModels;

namespace PermissionManagement.Services
{
    public interface IPermissionService
    {
        public Task<PermissionViewModel> addPermissionAsync(PermissionViewModel permission);
        public Task<permissionModel> getPermissionById(Guid idPermission);
        public Task<List<permissionModel>> getPermissionByEmployeeId(Guid idPermission);
        public Task<List<permissionModel>> getPermissionList();
        public Task<bool> updatePermission(Guid id, permissionModel permission);
        public Task<bool> deletePermission(Guid idPermission);
    }
}
