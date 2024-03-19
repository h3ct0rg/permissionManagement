using PermissionManagement.Helpers;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Model;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;

namespace PermissionManagement.CQRS.Query
{
    public class GetPermissionByEmployeeQuery
    {
        public Guid EmployeeId { get; set; }
        public GetPermissionByEmployeeQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }

    public class GetPermissionByEmployeeQueryHandler : IQueryHandler<GetPermissionByEmployeeQuery, List<permissionModel>>
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IKafkaService _kafkaService;

        public GetPermissionByEmployeeQueryHandler(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
        }

        public async Task<List<permissionModel>> HandleAsync(GetPermissionByEmployeeQuery query)
        {
            var permissionResponseList = await _permissionService.getPermissionByEmployeeId(query.EmployeeId);
            await _elasticSearchService.IndexDocumentAsync(permissionResponseList, stringConstants.permissionIndex, default);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.get), "test_topic");
            return permissionResponseList;
        }
    }
}
