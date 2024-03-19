using AutoMapper;
using PermissionManagement.Model;
using PermissionManagement.Repositories.UnitOfWork;
using PermissionManagement.Repository;
using PermissionManagement.Repository.PermissionRepository;
using PermissionManagement.ViewModels;
using Serilog;
using System.Security;

namespace PermissionManagement.Services
{
    public class PermissionService : BaseService, IPermissionService
    {
        private IPermissionRepository _permissionRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private Serilog.ILogger _logger;
        public PermissionService(
            IPermissionRepository permissionRepository,
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
                _logger.Information<PermissionViewModel>("permission added", permission);
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
                _logger.Information<permissionModel>("permission deletted", itemToDelete);
                return true;
            }
            else
            {
                _logger.Error<permissionModel>("permission to delete not found", itemToDelete);
                return false;
            }
        }

        public async Task<permissionModel> getPermissionById(Guid idPermission)
        {
            _logger.Information<Guid>("Get Permission By ID", idPermission);
            return await _permissionRepository.getByIDAsync(idPermission);
        }

        public async Task<List<permissionModel>> getPermissionList()
        {
            _logger.Information("Get full Permission List");
            return await _permissionRepository.GetAll();
        }

        public async Task<List<permissionModel>> getPermissionByEmployeeId(Guid employeeId)
        {
            _logger.Information<Guid>("Get Permission List By EmployeeId", employeeId);
            return await _permissionRepository.GetAllPermissionByEmployeeId(employeeId);
        }

        public async Task<bool> updatePermission(Guid id, permissionModel permission)
        {
            try
            {
                var itemToUpdate = await _permissionRepository.getByIDAsync(id);

                itemToUpdate.updatedDate = DateTime.Now;

                if (itemToUpdate != null)
                {
                    await BegginTransationAsync(_unitOfWork);
                    _permissionRepository.Update(itemToUpdate);
                    await SaveAsync(_unitOfWork);
                    _logger.Information<permissionModel>("permission updated", permission);
                    return true;
                }
            }
            catch (Exception ex)
            {
                await RollBackAsync(_unitOfWork);
                _logger.Error<permissionModel>(ex.Message, permission);
            }
            return false;
        }
    }
}
