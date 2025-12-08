using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.DonationGallery
{
    public class DonationGalleryDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventCode { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
    }

    public class DonationGalleryPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventCode { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
