
using PermissionManagement.Repositories.UnitOfWork;

namespace PermissionManagement.Services
{
    public class BaseService
    {
        public async Task SaveAsync(IUnitOfWork unitOfWork)
        {
            await unitOfWork.Commit();
        }

        public async Task BegginTransationAsync(IUnitOfWork unitOfWork)
        {
            await unitOfWork.BeginTransaction();
        }

        public async Task RollBackAsync(IUnitOfWork unitOfWork)
        {
            await unitOfWork.Rollback();
        }
    }
}
