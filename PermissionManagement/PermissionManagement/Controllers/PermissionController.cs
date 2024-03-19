using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionManagement.Helpers;
using PermissionManagement.Model;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;
using System.Threading;

namespace PermissionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService;
        private IElasticSearchService _elasticSearchService;
        private IKafkaService _kafkaService;
        private CancellationToken _cancellationToken;

        public PermissionController(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
            _cancellationToken = new CancellationToken();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var permissionResponseList = await _permissionService.getPermissionList();
            await _elasticSearchService.IndexDocumentAsync(permissionResponseList, stringConstants.permissionIndex, _cancellationToken);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.get), "test_topic");
            return Ok(permissionResponseList);
        }

        [HttpGet("getPermissionByEmployee")]
        public async Task<IActionResult> GetPermissionByEmployee([FromQuery] Guid employeeId)
        {
            var permissionResponseList = await _permissionService.getPermissionByEmployeeId(employeeId);
            await _elasticSearchService.IndexDocumentAsync(permissionResponseList, stringConstants.permissionIndex, _cancellationToken);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.get), "test_topic");
            return Ok(permissionResponseList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var permissionItem = await _permissionService.getPermissionById(id);
            await _elasticSearchService.IndexDocumentAsync(permissionItem, stringConstants.permissionIndex, _cancellationToken);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.get), "test_topic");
            return Ok(permissionItem);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PermissionViewModel permission)
        {
            var permissionResponse = await _permissionService.addPermissionAsync(permission);
            await _elasticSearchService.IndexDocumentAsync(permissionResponse, stringConstants.permissionIndex, _cancellationToken);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.request), "test_topic");
            return Ok(permissionResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, permissionModel permission)
        {
            var permissionResponse = await _permissionService.updatePermission(id, permission);
            await _elasticSearchService.IndexDocumentAsync(permission, stringConstants.permissionIndex, _cancellationToken);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.modify), "test_topic");
            return Ok(permissionResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var permissionResponse = await _permissionService.deletePermission(id);
            await _elasticSearchService.IndexDocumentAsync(new permissionModel { Id = id }, stringConstants.permissionIndex, _cancellationToken);
            return Ok(permissionResponse);
        }
    }
}
