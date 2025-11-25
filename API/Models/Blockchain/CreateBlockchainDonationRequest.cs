using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Blockchain
{
    public class CreateBlockchainDonationRequest
    {
        [Mandatory]
        [JsonPropertyName("donationId")]
        public string DonationId { get; set; } = string.Empty;

        [Mandatory]
        [MaxLength(255)]
        [JsonPropertyName("senderName")]
        public string SenderName { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;

        [MaxLength(500)]
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; } = string.Empty;
    }
}
