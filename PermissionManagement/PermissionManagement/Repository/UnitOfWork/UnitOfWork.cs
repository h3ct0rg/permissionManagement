using Microsoft.EntityFrameworkCore;
using PermissionManagement.Model;

namespace PermissionManagement.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly permissionModelContext _context;

        public UnitOfWork(permissionModelContext context)
        {
            _context = context;
        }

        public async Task BeginTransaction()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task Commit()
        {
            try
            {
                
                await _context.SaveChangesAsync();
                await _context.Database.CurrentTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task Rollback()
        {
            await _context.Database.CurrentTransaction.RollbackAsync();
        }
    }
}
