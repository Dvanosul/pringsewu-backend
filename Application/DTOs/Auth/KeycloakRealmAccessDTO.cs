using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Auth
{
    public class KeycloakRealmAccessDTO
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();
    }
}
