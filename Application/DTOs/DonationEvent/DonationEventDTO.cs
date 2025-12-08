using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.Category;

namespace Sindika.AspNet.app015.Application.DTOs.DonationEvent
{
    public class DonationEventDTO
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDTO? Category { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
