namespace PermissionManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
