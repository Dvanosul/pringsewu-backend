
namespace Sindika.AspNet.app015.Application.DTOs.RoleProfile
{
    public class RoleProfileParam
    {
        public Guid RoleId { get; set; }
        public List<Guid> ChangedProfiles { get; set; } = [];
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
