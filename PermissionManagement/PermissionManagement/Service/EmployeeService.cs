using AutoMapper;
using Serilog;
using PermissionManagement.ViewModels;
using PermissionManagement.Services;
using PermissionManagement.Repositories;
using PermissionManagement.Model;
using PermissionManagement.Repositories.UnitOfWork;

namespace PermissionManagement.Services
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private IRepository<EmployeeModel> _employeeRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private Serilog.ILogger _logger;
        public EmployeeService(
            IRepository<EmployeeModel> employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            Serilog.ILogger logger)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeViewModel> addEmployeeAsync(EmployeeViewModel employee)
        {
            try
            {
                await BegginTransationAsync(_unitOfWork);
                var employeeMap = _mapper.Map<EmployeeModel>(employee);
                employeeMap.createdDate = DateTime.Now;
                await _employeeRepository.AddAsync(employeeMap);
                await SaveAsync(_unitOfWork);
                employee = _mapper.Map<EmployeeViewModel>(employeeMap);
#if DEBUG
                _logger.Information<EmployeeViewModel>("employee added", employee);
#endif
                return employee;
            }
            catch (Exception ex)
            {
                await RollBackAsync(_unitOfWork);
                _logger.Error<EmployeeViewModel>(ex.Message, employee);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> deleteEmployee(Guid idemployee)
        {
            var itemToDelete = await _employeeRepository.getByIDAsync(idemployee);
            if (itemToDelete != null)
            {
                _employeeRepository.Delete(itemToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<EmployeeModel> getEmployeeByID(Guid idemployee)
        {
            return await _employeeRepository.getByIDAsync(idemployee);
        }

        public async Task<List<EmployeeModel>> getEmployeeList()
        {
            return await _employeeRepository.GetAll();
        }

        public async Task<bool> updateEmployee(Guid id, EmployeeModel employee)
        {
            try
            {
                var itemToUpdate = await _employeeRepository.getByIDAsync(id);
                itemToUpdate.name = employee.name != null ? employee.name : itemToUpdate.name;
                itemToUpdate.updatedDate = DateTime.Now;

                if (itemToUpdate != null)
                {
                    await BegginTransationAsync(_unitOfWork);
                    _employeeRepository.Update(itemToUpdate);
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
