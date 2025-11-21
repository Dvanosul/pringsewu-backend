
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.Menu
{
    public class CreateMenuRequest
    {
        [JsonPropertyName("parentMenu")]
        public Guid? ParentMenu { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        [KebabCase]
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [MinLength(3)]
        [MaxLength(255)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonPropertyName("pageId")]
        public Guid? PageId { get; set; }

        [JsonPropertyName("eventId")]
        public Guid? EventId { get; set; }
    }
}
