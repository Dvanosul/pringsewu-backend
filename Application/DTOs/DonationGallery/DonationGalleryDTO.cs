using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.DonationEvent;

namespace Sindika.AspNet.app015.Application.DTOs.DonationGallery
{
    public class DonationGalleryDTO
    {
        public Guid Id { get; set; }
        public Guid DonationEventId { get; set; }
        public DonationEventDTO? DonationEvent { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
