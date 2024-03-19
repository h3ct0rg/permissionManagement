using Microsoft.AspNetCore.Mvc;
using PermissionManagement.CQRS.Query;
using PermissionManagement.CQRS;
using PermissionManagement.Model;
using PermissionManagement.ViewModels;
using PermissionManagement.CQRS.Command;

namespace PermissionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IQueryHandler<GetPermissionListQuery, List<permissionModel>> _getPermissionListQueryHandler;
        private readonly IQueryHandler<GetPermissionByEmployeeQuery, List<permissionModel>> _getPermissionByEmployeeQueryHandler;
        private readonly IQueryHandler<GetPermissionByIdQuery, permissionModel> _getPermissionByIdQueryHandler;
        private readonly ICommandHandler<AddPermissionCommand, PermissionViewModel> _addPermissionCommand;
        private readonly ICommandHandler<UpdatePermissionCommand, bool> _updatePermissionCommand;
        private readonly ICommandHandler<DeletePermissionCommand, bool> _deletePermissionCommand;

        public PermissionController(
            IQueryHandler<GetPermissionListQuery, List<permissionModel>> getPermissionListQueryHandler,
            IQueryHandler<GetPermissionByEmployeeQuery, List<permissionModel>> getPermissionByEmployeeQueryHandler,
            IQueryHandler<GetPermissionByIdQuery, permissionModel> getPermissionByIdQueryHandler,
            ICommandHandler<AddPermissionCommand, PermissionViewModel> addPermissionCommand,
            ICommandHandler<UpdatePermissionCommand, bool> updatePermissionCommand,
            ICommandHandler<DeletePermissionCommand, bool> deletePermissionCommand
            )
        {
            _getPermissionListQueryHandler = getPermissionListQueryHandler;
            _getPermissionByEmployeeQueryHandler = getPermissionByEmployeeQueryHandler;
            _getPermissionByIdQueryHandler = getPermissionByIdQueryHandler;
            _addPermissionCommand = addPermissionCommand;
            _updatePermissionCommand = updatePermissionCommand;
            _deletePermissionCommand = deletePermissionCommand;            
        }

        /// <summary>
        /// Retrieves a list of permissions.
        /// </summary>
        /// <returns>A list of permissions.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetPermissionListQuery();
            var response = await _getPermissionListQueryHandler.HandleAsync(query);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves permissions associated with a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>Permissions associated with the employee.</returns>
        [HttpGet("getPermissionByEmployee")]
        public async Task<IActionResult> GetPermissionByEmployee([FromQuery] Guid employeeId)
        {
            var query = new GetPermissionByEmployeeQuery(employeeId);
            var response = await _getPermissionByEmployeeQueryHandler.HandleAsync(query);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission.</param>
        /// <returns>The permission.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var query = new GetPermissionByIdQuery(id);
            var response = await _getPermissionByIdQueryHandler.HandleAsync(query);
            return Ok(response);
        }

        /// <summary>
        /// Adds a new permission.
        /// </summary>
        /// <param name="permission">The permission to add.</param>
        /// <returns>The added permission.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(PermissionViewModel permission)
        {
            var query = new AddPermissionCommand(permission);
            var response = await _addPermissionCommand.HandleAsync(query);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="id">The ID of the permission to update.</param>
        /// <param name="permission">The updated permission data.</param>
        /// <returns>The updated permission.</returns>
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, permissionModel permission)
        {
            var query = new UpdatePermissionCommand(id, permission);
            var response = await _updatePermissionCommand.HandleAsync(query);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new DeletePermissionCommand(id);
            var response = await _deletePermissionCommand.HandleAsync(query);
            return Ok(response);
        }
    }
}
