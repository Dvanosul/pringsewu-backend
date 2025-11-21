
namespace Sindika.AspNet.app015.Application.DTOs.Developer
{
    public class UpdateDeveloperParam
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsSuper { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public IEnumerable<Guid> ChangedRoles { get; set; } = [];
    }
}
