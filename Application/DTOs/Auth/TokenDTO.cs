using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Auth
{
    public class TokenDTO
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("role_active")]
        public string? RoleActive { get; set; }

        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        [JsonPropertyName("roles")]
        public List<string>? Roles { get; set; }
    }
}
