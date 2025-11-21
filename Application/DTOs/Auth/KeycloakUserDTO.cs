using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Auth
{
    public class KeycloakUserDTO
    {
        [JsonPropertyName("allowed-origins")]
        public List<string> AllowedOrigins { get; set; } = new();

        [JsonPropertyName("realm_access")]
        public KeycloakRealmAccessDTO RealmAccess { get; set; } = new();

        [JsonPropertyName("resource_access")]
        public Dictionary<string, KeycloakResourceAccessDTO> ResourceAccess { get; set; } = new();

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("email_verified")]
        public bool IsEmailVerified { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("preferred_username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("given_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("family_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}
