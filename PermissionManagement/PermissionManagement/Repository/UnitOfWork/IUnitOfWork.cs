namespace PermissionManagement.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();
    }
}
