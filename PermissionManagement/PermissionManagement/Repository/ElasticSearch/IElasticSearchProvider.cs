using Nest;

namespace PermissionManagement.Repository.ElasticSearch
{
    public interface IElasticSearchProvider
    {
        ElasticClient GetClient();
    }
}
