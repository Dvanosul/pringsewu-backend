using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.PageEvent
{
    public class PageEventPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool HasEvent { get; set; }
    }
}
