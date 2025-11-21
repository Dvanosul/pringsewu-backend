
namespace Sindika.AspNet.app015.Application.DTOs.Employee
{
    public class EmployeeRoleParam
    {
        public Guid EmployeeId { get; set; }
        public Guid RoleId { get; set; }
        public bool IsEnabled { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
