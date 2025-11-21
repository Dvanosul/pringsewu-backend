
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.User
{
    public class CreateUserRequest
    {
        [MinLength(3)]
        [MaxLength(255)]
        [TitleCase]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [MinLength(3)]
        [MaxLength(255)]
        [Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email Format")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("languageId")]
        public Guid? LanguageId { get; set; }

        [JsonPropertyName("zoneId")]
        public Guid? ZoneId { get; set; }

        [JsonPropertyName("developerId")]
        public Guid? DeveloperId { get; set; }

        [JsonPropertyName("employeeId")]
        public Guid? EmployeeId { get; set; }

        [JsonPropertyName("isSuper")]
        public bool IsSuper { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }
    }
}
