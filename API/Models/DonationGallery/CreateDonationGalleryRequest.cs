using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.DonationGallery
{
    public class CreateDonationGalleryRequest
    {
        [Mandatory]
        public Guid DonationEventId { get; set; }

        [Mandatory]
        public required IFormFile ImgUrl { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
