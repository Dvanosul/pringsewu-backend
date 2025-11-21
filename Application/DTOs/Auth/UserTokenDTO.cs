using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Auth
{
    public class UserTokenDTO
    {
        [JsonPropertyName("exp")]
        public long Exp { get; set; }

        [JsonPropertyName("iat")]
        public long Iat { get; set; }

        [JsonPropertyName("auth_time")]
        public long AuthTime { get; set; }

        [JsonPropertyName("jti")]
        public string Jti { get; set; } = string.Empty;

        [JsonPropertyName("iss")]
        public string Iss { get; set; } = string.Empty;

        [JsonPropertyName("aud")]
        public List<string> Aud { get; set; } = new();

        [JsonPropertyName("sub")]
        public string Sub { get; set; } = string.Empty;

        [JsonPropertyName("typ")]
        public string Typ { get; set; } = string.Empty;

        [JsonPropertyName("azp")]
        public string Azp { get; set; } = string.Empty;

        [JsonPropertyName("sid")]
        public string Sid { get; set; } = string.Empty;

        [JsonPropertyName("acr")]
        public string Acr { get; set; } = string.Empty;

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
