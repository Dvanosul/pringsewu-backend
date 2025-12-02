using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Blockchain
{
    public class CreateBlockchainGalleryRequest
    {
        [Mandatory]
        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("image")]
        public required IFormFile Image { get; set; }

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateBlockchainGalleryRequest
    {
        [Mandatory]
        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
