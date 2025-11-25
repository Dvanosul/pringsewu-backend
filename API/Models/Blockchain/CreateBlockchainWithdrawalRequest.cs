using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Blockchain
{
    public class CreateBlockchainWithdrawalRequest
    {
        [Mandatory]
        [JsonPropertyName("withdrawalId")]
        public string WithdrawalId { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("eventCode")]
        public string EventCode { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;

        [Mandatory]
        [MaxLength(255)]
        [JsonPropertyName("withdrawBy")]
        public string WithdrawBy { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("dateTime")]
        public string DateTime { get; set; } = string.Empty;
    }
}
