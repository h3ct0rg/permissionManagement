namespace PermissionManagement.Service.ElasticSearch
{
    public interface IElasticSearchService
    {
        Task<bool> IndexDocumentAsync<T>(T document, string indexName, CancellationToken cancellationToken) where T : class;
    }
}
