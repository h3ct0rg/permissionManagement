using Nest;

namespace PermissionManagement.Repository.ElasticSearch
{
    public class ElasticSearchProvider : IElasticSearchProvider
    {
        private readonly ElasticClient _client;

        public ElasticSearchProvider(Uri uri)
        {
            var settings = new ConnectionSettings(uri).DefaultIndex("permission_index");
            _client = new ElasticClient(settings);
        }
        public ElasticClient GetClient()
        {
            return _client;
        }
    }
}
