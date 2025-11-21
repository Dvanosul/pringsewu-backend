
namespace Sindika.AspNet.app015.Application.DTOs.Role
{
    public class RoleParam
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid UserTypeId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
