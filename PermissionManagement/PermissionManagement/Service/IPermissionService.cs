using PermissionManagement.Model;
using PermissionManagement.ViewModels;

namespace PermissionManagement.Services
{
    public interface IPermissionService
    {
        public Task<PermissionViewModel> addPermissionAsync(PermissionViewModel permission);
        public Task<permissionModel> getPermissionByID(Guid idPermission);
        public Task<List<permissionModel>> getPermissionList();
        public Task<bool> updatePermission(Guid id, permissionModel permission);
        public Task<bool> deletePermission(Guid idPermission);
    }
}
