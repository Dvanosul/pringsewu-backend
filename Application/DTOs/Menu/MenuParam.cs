
namespace Sindika.AspNet.app015.Application.DTOs.Menu
{
    public class MenuParam
    {
        public Guid? ParentMenu { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Guid? PageId { get; set; }
        public Guid? EventId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
