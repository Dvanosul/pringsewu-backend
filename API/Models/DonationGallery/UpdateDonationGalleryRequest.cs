using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.DonationGallery
{
    public class UpdateDonationGalleryRequest
    {
        [Mandatory]
        [JsonPropertyName("eventId")]
        public Guid EventId { get; set; }

        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
