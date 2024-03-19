using PermissionManagement.Helpers;
using PermissionManagement.Model;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Services;

namespace PermissionManagement.CQRS.Command
{
    public class DeletePermissionCommand
    {
        public Guid Id { get; set; }
        public DeletePermissionCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeletePermissionCommandHandler : ICommandHandler<DeletePermissionCommand, bool>
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticSearchService _elasticSearchService;

        public DeletePermissionCommandHandler(IPermissionService permissionService, IElasticSearchService elasticSearchService)
        {
            _permissionService = permissionService;
            _elasticSearchService = elasticSearchService;
        }

        public async Task<bool> HandleAsync(DeletePermissionCommand command)
        {
            var permissionResponse = await _permissionService.deletePermission(command.Id);
            await _elasticSearchService.IndexDocumentAsync(new permissionModel { Id = command.Id }, stringConstants.permissionIndex, default);
            return permissionResponse;
        }
    }
}
