using PermissionManagement.Helpers;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Model;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;

namespace PermissionManagement.CQRS.Query
{
    public class GetPermissionByIdQuery
    {
        public Guid Id { get; set; }
        public GetPermissionByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetPermissionByIdQueryHandler : IQueryHandler<GetPermissionByIdQuery, permissionModel>
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IKafkaService _kafkaService;

        public GetPermissionByIdQueryHandler(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
        }

        public async Task<permissionModel> HandleAsync(GetPermissionByIdQuery query)
        {
            var permissionItem = await _permissionService.getPermissionById(query.Id);
            await _elasticSearchService.IndexDocumentAsync(permissionItem, stringConstants.permissionIndex, default);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.get), "test_topic");
            return permissionItem;
        }
    }
}
