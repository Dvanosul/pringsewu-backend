using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Page
{
    public class PageCustomEventDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}
