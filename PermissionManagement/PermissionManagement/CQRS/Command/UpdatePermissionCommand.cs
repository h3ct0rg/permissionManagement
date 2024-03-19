using PermissionManagement.Helpers;
using PermissionManagement.Model.Kafka;
using PermissionManagement.Model;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;

namespace PermissionManagement.CQRS.Command
{
    public class UpdatePermissionCommand
    {
        public Guid Id { get; set; }
        public permissionModel Permission { get; set; }
        public UpdatePermissionCommand(Guid id, permissionModel permission)
        {
            Id = id;
            Permission = permission;
        }
    }

    public class UpdatePermissionCommandHandler : ICommandHandler<UpdatePermissionCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IKafkaService _kafkaService;

        public UpdatePermissionCommandHandler(IPermissionService permissionService, IElasticSearchService elasticSearchService, IKafkaService kafkaService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
            _kafkaService = kafkaService;
        }

        public async Task<bool> HandleAsync(UpdatePermissionCommand command)
        {
            var permissionResponse = await _permissionService.updatePermission(command.Id, command.Permission);
            await _elasticSearchService.IndexDocumentAsync(command.Permission, stringConstants.permissionIndex, default);
            _kafkaService.ProduceMessage(new KafkaMessageModel(EnumConstants.enumOperation.modify), "test_topic");
            return permissionResponse;
        }
    }
}
