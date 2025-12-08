using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.DonationEvent
{
    public class UpdateDonationEventRequest
    {
        [Mandatory]
        [MinLength(3)]
        [MaxLength(255)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        [Mandatory]
        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [Mandatory]
        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

    public class UpdateDonationEventStatusRequest
    {
        [Mandatory]
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}
