namespace PermissionManagement.ViewModels
{
    public class PermissionViewModel
    {
        public Guid Id { get; set; }
        public Guid IdEmployee { get; set; }
        public Guid IdPermissionType { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
