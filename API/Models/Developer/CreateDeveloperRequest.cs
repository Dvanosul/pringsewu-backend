
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.Phone;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.Developer
{
    public class CreateDeveloperRequest
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

        [ValidPhoneFormat]
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("roles")]
        public IEnumerable<Guid> Roles { get; set; } = [];
    }
}
