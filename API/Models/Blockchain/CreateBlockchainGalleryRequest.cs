using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Blockchain
{
    public class CreateBlockchainGalleryRequest
    {
        [Mandatory]
        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; } = string.Empty;

        [Mandatory]
        [MaxLength(500)]
        [JsonPropertyName("imageURL")]
        public string ImageURL { get; set; } = string.Empty;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateBlockchainGalleryRequest
    {
        [Mandatory]
        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; } = string.Empty;

        [Mandatory]
        [MaxLength(500)]
        [JsonPropertyName("imageURL")]
        public string ImageURL { get; set; } = string.Empty;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
