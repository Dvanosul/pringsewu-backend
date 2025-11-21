using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.API.Models.UserUserType
{
    public class UserUserTypeRequest
    {
        [JsonPropertyName("roleId")]
        public Guid RoleId { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }
    }
}
