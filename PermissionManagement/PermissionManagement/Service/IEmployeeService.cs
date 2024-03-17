using PermissionManagement.Model;
using PermissionManagement.ViewModels;

namespace PermissionManagement.Services
{
    public interface IEmployeeService
    {
        public Task<EmployeeViewModel> addEmployeeAsync(EmployeeViewModel employee);
        public Task<EmployeeModel> getEmployeeByID(Guid idemployee);
        public Task<List<EmployeeModel>> getEmployeeList();
        public Task<bool> updateEmployee(Guid id, EmployeeModel employee);
        public Task<bool> deleteEmployee(Guid idemployee);
    }
}
