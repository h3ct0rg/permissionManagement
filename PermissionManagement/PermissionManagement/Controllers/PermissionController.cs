using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionManagement.Model;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;

namespace PermissionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var permissionList = await _permissionService.getPermissionList();
            return Ok(permissionList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var permissionList = await _permissionService.getPermissionByID(id);
            return Ok(permissionList);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PermissionViewModel permission)
        {
            var permissionResponse = await _permissionService.addPermissionAsync(permission);
            return Ok(permissionResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, permissionModel permission)
        {
            var permissionList = await _permissionService.updatePermission(id, permission);
            return Ok(permissionList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var permissionList = await _permissionService.deletePermission(id);
            return Ok(permissionList);
        }
    }
}
