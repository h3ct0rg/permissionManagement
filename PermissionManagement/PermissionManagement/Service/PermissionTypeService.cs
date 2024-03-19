using AutoMapper;
using Serilog;
using PermissionManagement.ViewModels;
using PermissionManagement.Services;
using PermissionManagement.Model;
using PermissionManagement.Repositories.UnitOfWork;
using PermissionManagement.Repository.BaseRepository;

namespace PermissionManagement.Services
{
    public class PermissionTypeService : BaseService, IPermissionTypeService
    {
        private IRepository<PermissionTypeModel> _PermissionTypeRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private Serilog.ILogger _logger;
        public PermissionTypeService(
            IRepository<PermissionTypeModel> PermissionTypeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            Serilog.ILogger logger)
        {
            _PermissionTypeRepository = PermissionTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PermissionTypeViewModel> addPermissionTypeAsync(PermissionTypeViewModel PermissionType)
        {
            try
            {
                await BegginTransationAsync(_unitOfWork);
                var PermissionTypeMap = _mapper.Map<PermissionTypeModel>(PermissionType);
                PermissionTypeMap.createdDate = DateTime.Now;
                await _PermissionTypeRepository.AddAsync(PermissionTypeMap);
                await SaveAsync(_unitOfWork);
                PermissionType = _mapper.Map<PermissionTypeViewModel>(PermissionTypeMap);
#if DEBUG
                _logger.Information<PermissionTypeViewModel>("PermissionType added", PermissionType);
#endif
                return PermissionType;
            }
            catch (Exception ex)
            {
                await RollBackAsync(_unitOfWork);
                _logger.Error<PermissionTypeViewModel>(ex.Message, PermissionType);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> deletePermissionType(Guid idPermissionType)
        {
            var itemToDelete = await _PermissionTypeRepository.getByIDAsync(idPermissionType);
            if (itemToDelete != null)
            {
                _PermissionTypeRepository.Delete(itemToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PermissionTypeModel> getPermissionTypeByID(Guid idPermissionType)
        {
            return await _PermissionTypeRepository.getByIDAsync(idPermissionType);
        }

        public async Task<List<PermissionTypeModel>> getPermissionTypeList()
        {
            return await _PermissionTypeRepository.GetAll();
        }

        public async Task<bool> updatePermissionType(Guid id, PermissionTypeModel PermissionType)
        {
            try
            {
                var itemToUpdate = await _PermissionTypeRepository.getByIDAsync(id);
                itemToUpdate.name = PermissionType.name != null ? PermissionType.name : itemToUpdate.name;
                itemToUpdate.updatedDate = DateTime.Now;

                if (itemToUpdate != null)
                {
                    await BegginTransationAsync(_unitOfWork);
                    _PermissionTypeRepository.Update(itemToUpdate);
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
