using Nest;
using PermissionManagement.Repository.ElasticSearch;

namespace PermissionManagement.Service.ElasticSearch
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticSearchProvider _elasticsearchClientProvider;

        public ElasticSearchService(IElasticSearchProvider elasticsearchClientProvider)
        {
            _elasticsearchClientProvider = elasticsearchClientProvider;
        }

        public async Task<bool> IndexDocumentAsync<T>(T document, string indexName, CancellationToken cancellationToken) where T : class
        {
            try
            {
                var client = _elasticsearchClientProvider.GetClient();

                var indexRequest = new IndexRequest<T>(document, indexName);

                var response = await client.IndexDocumentAsync(indexRequest, cancellationToken);

                return response.IsValid;
            }
            catch (Exception ex)
            {
                var res = ex.Message;
                return false;
            }
        }
    }
}
