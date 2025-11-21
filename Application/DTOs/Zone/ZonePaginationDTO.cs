
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Zone
{
    public class ZonePaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Offset { get; set; } = string.Empty;
        public int? OffsetMinute { get; set; }
        public string Label { get; set; } = string.Empty;
        public string TZCode { get; set; } = string.Empty;
    }
}
