using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Page
{
    public class PageWithEventsIdPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public IEnumerable<Guid> Events { get; set; } = [];
        public long EnabledCustomEventsCount { get; set; }
        public long CustomEventsCount { get; set; }
    }
}
