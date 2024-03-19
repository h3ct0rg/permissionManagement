using Microsoft.EntityFrameworkCore;
using PermissionManagement.Model;
using PermissionManagement.Repository.BaseRepository;

namespace PermissionManagement.Repository.PermissionRepository
{
    public class PermissionRepository : RepositoryBase<permissionModel>, IPermissionRepository
    {
        public PermissionRepository(permissionModelContext context) : base(context) { }

        public Task<List<permissionModel>> GetAllPermissionByEmployeeId(Guid employeeId)
        {
            return _dbSet.Where(x => x.idEmployee == employeeId).ToListAsync();
        }
        public Task AddAsync(permissionModel permissionMap)
        {
            return base.AddAsync(permissionMap);
        }

        public void Delete(permissionModel itemToDelete)
        {
            base.Delete(itemToDelete);
        }

        Task<permissionModel> IPermissionRepository.getByIDAsync(Guid idPermission)
        {
            return getByIDAsync(idPermission);
        }
    }
}
