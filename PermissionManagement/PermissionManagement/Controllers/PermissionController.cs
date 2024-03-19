using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionManagement.CQRS.Query;
using PermissionManagement.CQRS;
using PermissionManagement.Helpers;
using PermissionManagement.Model;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;
using System.Threading;
using PermissionManagement.CQRS.Command;

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
        private readonly IQueryHandler<GetPermissionListQuery, List<permissionModel>> _getPermissionListQueryHandler;
        private readonly IQueryHandler<GetPermissionByEmployeeQuery, List<permissionModel>> _getPermissionByEmployeeQueryHandler;
        private readonly IQueryHandler<GetPermissionByIdQuery, permissionModel> _getPermissionByIdQueryHandler;

        public PermissionController(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService,
            IQueryHandler<GetPermissionListQuery, List<permissionModel>> getPermissionListQueryHandler,
            IQueryHandler<GetPermissionByEmployeeQuery, List<permissionModel>> getPermissionByEmployeeQueryHandler,
            IQueryHandler<GetPermissionByIdQuery, permissionModel> getPermissionByIdQueryHandler)
        {
            _getPermissionListQueryHandler = getPermissionListQueryHandler;
            _getPermissionByEmployeeQueryHandler = getPermissionByEmployeeQueryHandler;
            _getPermissionByIdQueryHandler = getPermissionByIdQueryHandler;

            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
            _cancellationToken = new CancellationToken();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetPermissionListQuery();
            var response = await _getPermissionListQueryHandler.HandleAsync(query);
            return Ok(response);
        }

        [HttpGet("getPermissionByEmployee")]
        public async Task<IActionResult> GetPermissionByEmployee([FromQuery] Guid employeeId)
        {
            var query = new GetPermissionByEmployeeQuery(employeeId);
            var response = await _getPermissionByEmployeeQueryHandler.HandleAsync(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var query = new GetPermissionByIdQuery(id);
            var response = await _getPermissionByIdQueryHandler.HandleAsync(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PermissionViewModel permission)
        {
            //var query = new AddPermissionCommand(permission);
            //var response = await _addPermissionCommand.HandleAsync(query);
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
