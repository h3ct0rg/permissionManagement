using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionManagement.Model;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;

namespace employeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        private IEmployeeService _employeeService;

        public EmployeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employeeList = await _employeeService.getEmployeeList();
            return Ok(employeeList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var employeeList = await _employeeService.getEmployeeByID(id);
            return Ok(employeeList);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmployeeViewModel employee)
        {
            var employeeResponse = await _employeeService.addEmployeeAsync(employee);
            return Ok(employeeResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, EmployeeModel employee)
        {
            var employeeList = await _employeeService.updateEmployee(id, employee);
            return Ok(employeeList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employeeList = await _employeeService.deleteEmployee(id);
            return Ok(employeeList);
        }
    }
}
