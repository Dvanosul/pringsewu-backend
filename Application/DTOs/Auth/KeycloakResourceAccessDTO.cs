using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Auth
{
    public class KeycloakResourceAccessDTO
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();
    }
}
