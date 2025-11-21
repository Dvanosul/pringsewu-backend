using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.CustomEvent
{
    public class CustomEventPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int AppliedPages { get; set; }
    }
}
