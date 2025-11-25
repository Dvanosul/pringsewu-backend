using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Blockchain
{
    public class CreateBlockchainEventRequest
    {
        [Mandatory]
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [Mandatory]
        [MaxLength(255)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("startDate")]
        public string StartDate { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("endDate")]
        public string EndDate { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public string IsActive { get; set; } = "true";
    }

    public class UpdateBlockchainEventStatusRequest
    {
        [Mandatory]
        [JsonPropertyName("isActive")]
        public string IsActive { get; set; } = string.Empty;
    }

    public class UpdateBlockchainEventRequest
    {
        [Mandatory]
        [MaxLength(255)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("startDate")]
        public string StartDate { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("endDate")]
        public string EndDate { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("isActive")]
        public string IsActive { get; set; } = string.Empty;
    }
}
