using Microsoft.EntityFrameworkCore;

namespace PermissionManagement.Model
{
    public class permissionModelContext : DbContext
    {
        public permissionModelContext(DbContextOptions<permissionModelContext> options) : base(options) { }
        public DbSet<EmployeeModel> Employee { get; set; }
        public DbSet<permissionModel> Permission { get; set; }
        public DbSet<PermissionTypeModel> PersonPermission { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<permissionModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<PermissionTypeModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");
        }
    }
}
