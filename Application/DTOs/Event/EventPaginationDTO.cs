using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Event
{
    public class EventPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = true;
    }
}
