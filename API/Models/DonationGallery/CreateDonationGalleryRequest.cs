using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.DonationGallery
{
    public class CreateDonationGalleryRequest
    {
        [Mandatory]
        [JsonPropertyName("eventId")]
        public Guid EventId { get; set; }

        [Mandatory]
        [JsonPropertyName("image")]
        public required IFormFile Image { get; set; }

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
