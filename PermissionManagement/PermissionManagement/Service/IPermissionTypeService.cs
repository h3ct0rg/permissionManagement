using PermissionManagement.Model;
using PermissionManagement.ViewModels;

namespace PermissionManagement.Services
{
    public interface IPermissionTypeService
    {
        public Task<PermissionTypeViewModel> addPermissionTypeAsync(PermissionTypeViewModel PermissionType);
        public Task<PermissionTypeModel> getPermissionTypeByID(Guid idPermissionType);
        public Task<List<PermissionTypeModel>> getPermissionTypeList();
        public Task<bool> updatePermissionType(Guid id, PermissionTypeModel PermissionType);
        public Task<bool> deletePermissionType(Guid idPermissionType);
    }
}
