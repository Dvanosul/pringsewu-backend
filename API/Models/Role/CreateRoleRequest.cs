
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.Role
{
    public class CreateRoleRequest
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

        [JsonPropertyName("userTypeId")]
        public Guid UserTypeId { get; set; }
    }
}
