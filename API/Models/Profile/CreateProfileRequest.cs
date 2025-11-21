
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.Profile
{
    public class CreateProfileRequest
    {
        [MinLength(3)]
        [MaxLength(255)]
        [TitleCase]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [MinLength(3)]
        [MaxLength(255)]
        [KebabCase]
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [MinLength(3)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
