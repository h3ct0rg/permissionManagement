using PermissionManagement.Helpers;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;

namespace PermissionManagement.CQRS.Command
{
    public class AddPermissionCommand
    {
        public PermissionViewModel Permission { get; set; }
        public AddPermissionCommand(PermissionViewModel permission)
        {
            Permission = permission;
        }
    }

    public class AddPermissionCommandHandler : ICommandHandler<AddPermissionCommand, PermissionViewModel>
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IKafkaService _kafkaService;

        public AddPermissionCommandHandler(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
        }

        public async Task<PermissionViewModel> HandleAsync(AddPermissionCommand command)
        {
            var permissionResponse = await _permissionService.addPermissionAsync(command.Permission);
            await _elasticSearchService.IndexDocumentAsync(permissionResponse, stringConstants.permissionIndex, default);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.request), "test_topic");
            return permissionResponse;
        }
    }
}
