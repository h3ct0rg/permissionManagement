using PermissionManagement.Model;

namespace PermissionManagement.Repository.PermissionRepository
{
    public interface IPermissionRepository
    {
        Task AddAsync(permissionModel permissionMap);
        void Delete(permissionModel itemToDelete);
        Task<List<permissionModel>> GetAll();
        Task<List<permissionModel>> GetAllPermissionByEmployeeId(Guid employeeId);
        Task<permissionModel> getByIDAsync(Guid idPermission);
        void Update(permissionModel itemToUpdate);
    }
}
