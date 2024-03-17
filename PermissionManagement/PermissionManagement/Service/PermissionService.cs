using AutoMapper;
using PermissionManagement.Model;
using PermissionManagement.Repositories;
using PermissionManagement.Repositories.UnitOfWork;
using PermissionManagement.ViewModels;
using Serilog;

namespace PermissionManagement.Services
{
    public class PermissionService : BaseService, IPermissionService
    {
        private IRepository<permissionModel> _permissionRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private Serilog.ILogger _logger;
        public PermissionService(
            IRepository<permissionModel> permissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            Serilog.ILogger logger)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PermissionViewModel> addPermissionAsync(PermissionViewModel permission)
        {
            try
            {
                await BegginTransationAsync(_unitOfWork);
                var permissionMap = _mapper.Map<permissionModel>(permission);
                permissionMap.createdDate = DateTime.Now;
                await _permissionRepository.AddAsync(permissionMap);
                await SaveAsync(_unitOfWork);
                permission = _mapper.Map<PermissionViewModel>(permissionMap);
#if DEBUG
                _logger.Information<PermissionViewModel>("permission added", permission);
#endif
                return permission;
            }
            catch (Exception ex)
            {
                await RollBackAsync(_unitOfWork);
                _logger.Error<PermissionViewModel>(ex.Message, permission);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> deletePermission(Guid idPermission)
        {
            var itemToDelete = await _permissionRepository.getByIDAsync(idPermission);
            if (itemToDelete != null)
            {
                _permissionRepository.Delete(itemToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<permissionModel> getPermissionByID(Guid idPermission)
        {
            return await _permissionRepository.getByIDAsync(idPermission);
        }

        public async Task<List<permissionModel>> getPermissionList()
        {
            return await _permissionRepository.GetAll();
        }

        public async Task<bool> updatePermission(Guid id, permissionModel permission)
        {
            //add DTO parse
            try
            {
                var itemToUpdate = await _permissionRepository.getByIDAsync(id);

                itemToUpdate.updatedDate = DateTime.Now;

                if (itemToUpdate != null)
                {
                    await BegginTransationAsync(_unitOfWork);
                    _permissionRepository.Update(itemToUpdate);
                    await SaveAsync(_unitOfWork);
                    return true;
                }
            }
            catch (Exception ex)
            {
                await RollBackAsync(_unitOfWork);
            }
            return false;
        }
    }
}
