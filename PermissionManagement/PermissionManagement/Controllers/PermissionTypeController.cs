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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var PermissionTypeeList = await _PermissionTypeService.getPermissionTypeList();
            return Ok(PermissionTypeeList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var PermissionTypeeList = await _PermissionTypeService.getPermissionTypeByID(id);
            return Ok(PermissionTypeeList);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PermissionTypeViewModel PermissionTypee)
        {
            var PermissionTypeeResponse = await _PermissionTypeService.addPermissionTypeAsync(PermissionTypee);
            return Ok(PermissionTypeeResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, PermissionTypeModel PermissionTypee)
        {
            var PermissionTypeeList = await _PermissionTypeService.updatePermissionType(id, PermissionTypee);
            return Ok(PermissionTypeeList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var PermissionTypeeList = await _PermissionTypeService.deletePermissionType(id);
            return Ok(PermissionTypeeList);
        }
    }
}
