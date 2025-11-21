
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Page
{
    public class PageEventChangeRequest
    {

        [Mandatory]
        [JsonPropertyName("pageId")]
        public Guid PageId { get; set; }

        [Mandatory]
        [JsonPropertyName("eventId")]
        public Guid EventId { get; set; }
    }
}
