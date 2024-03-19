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

        /// <summary>
        /// Retrieves the list of employees.
        /// </summary>
        /// <returns>A list of employees.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employeeList = await _employeeService.getEmployeeList();
            return Ok(employeeList);
        }

        /// <summary>
        /// Retrieves an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>The employee information.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var employeeList = await _employeeService.getEmployeeByID(id);
            return Ok(employeeList);
        }

        /// <summary>
        /// Adds a new employee.
        /// </summary>
        /// <param name="employee">The employee to add.</param>
        /// <returns>The added employee.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(EmployeeViewModel employee)
        {
            var employeeResponse = await _employeeService.addEmployeeAsync(employee);
            return Ok(employeeResponse);
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employee">The updated employee data.</param>
        /// <returns>The updated employee.</returns>
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, EmployeeModel employee)
        {
            var employeeList = await _employeeService.updateEmployee(id, employee);
            return Ok(employeeList);
        }

        /// <summary>
        /// Deletes an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employeeList = await _employeeService.deleteEmployee(id);
            return Ok(employeeList);
        }
    }
}
