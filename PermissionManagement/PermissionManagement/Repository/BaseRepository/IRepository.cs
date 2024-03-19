namespace PermissionManagement.Repository.BaseRepository
{
    public interface IRepository<T>
    {
        Task<T> getByIDAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<List<T>> GetAll();
    }
}
