using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Page
{
    public class PageWithEventPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long EnabledCustomEventsCount { get; set; }
        public long CustomEventsCount { get; set; }
        public Dictionary<Guid, PageEventDTO> Events { get; set; } = [];
    }
}
