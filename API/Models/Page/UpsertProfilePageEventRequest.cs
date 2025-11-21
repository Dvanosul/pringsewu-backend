
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Page
{
    public class UpsertProfilePageEventRequest
    {
        [Mandatory]
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [Mandatory]
        [JsonPropertyName("changedPageEvents")]
        public IEnumerable<PageEventChangeRequest> ChangedPageEvents { get; set; } = null!;
    }
}
