
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.CustomEvent
{
    public class UpdateCustomEventRequest
    {
        [Mandatory]
        [MinLength(3)]
        [KebabCase]
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [Mandatory]
        [MinLength(3)]
        [MaxLength(255)]
        [TitleCase]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("changedAppliedPages")]
        public IEnumerable<Guid> ChangedAppliedPages { get; set; } = [];
    }
}
