using PermissionManagement.Helpers;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Model;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;

namespace PermissionManagement.CQRS.Query
{
    public class GetPermissionListQuery
    {
    }

    public class GetPermissionListQueryHandler : IQueryHandler<GetPermissionListQuery, List<permissionModel>>
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IKafkaService _kafkaService;

        public GetPermissionListQueryHandler(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
        }

        public async Task<List<permissionModel>> HandleAsync(GetPermissionListQuery query)
        {
            var permissionResponseList = await _permissionService.getPermissionList();
            await _elasticSearchService.IndexDocumentAsync(permissionResponseList, stringConstants.permissionIndex, default);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.get), "test_topic");
            return permissionResponseList;
        }
    }
}
