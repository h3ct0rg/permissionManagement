using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionManagement.Model;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;

namespace PermissionTypeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionTypeController : ControllerBase
    {
        private IPermissionTypeService _PermissionTypeService;

        public PermissionTypeController(IPermissionTypeService PermissionTypeService)
        {
            _PermissionTypeService = PermissionTypeService;
        }

        /// <summary>
        /// Retrieves the list of permission types.
        /// </summary>
        /// <returns>A list of permission types.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var PermissionTypeeList = await _PermissionTypeService.getPermissionTypeList();
            return Ok(PermissionTypeeList);
        }

        /// <summary>
        /// Retrieves a permission type by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission type.</param>
        /// <returns>The permission type information.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var PermissionTypeeList = await _PermissionTypeService.getPermissionTypeByID(id);
            return Ok(PermissionTypeeList);
        }

        /// <summary>
        /// Adds a new permission type.
        /// </summary>
        /// <param name="permissionType">The permission type to add.</param>
        /// <returns>The added permission type.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(PermissionTypeViewModel PermissionTypee)
        {
            var PermissionTypeeResponse = await _PermissionTypeService.addPermissionTypeAsync(PermissionTypee);
            return Ok(PermissionTypeeResponse);
        }

        /// <summary>
        /// Updates an existing permission type.
        /// </summary>
        /// <param name="id">The ID of the permission type to update.</param>
        /// <param name="permissionType">The updated permission type data.</param>
        /// <returns>The updated permission type.</returns>
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, PermissionTypeModel PermissionTypee)
        {
            var PermissionTypeeList = await _PermissionTypeService.updatePermissionType(id, PermissionTypee);
            return Ok(PermissionTypeeList);
        }

        /// <summary>
        /// Deletes a permission type by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission type to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var PermissionTypeeList = await _PermissionTypeService.deletePermissionType(id);
            return Ok(PermissionTypeeList);
        }
    }
}
