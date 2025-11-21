
namespace Sindika.AspNet.app015.Application.DTOs.Developer
{
    public class CreateDeveloperParam
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsSuper { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public IEnumerable<Guid> Roles { get; set; } = [];
    }
}
